using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;
using System;

public class RestaurantManagerChallenge : RestaurantManager{
	// our satisfaction ai 
	private ChallengeAI challengeAI;
	
	ImmutableDataChallenge chall;
	int interval = 0;
	

	public override void Init() {
		sickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		challengeAI = new ChallengeAI();
	}


	private void RunSetUp() {
		chall = DataLoaderChallenge.GetData(DataManager.Instance.GetChallenge());
		currCusSet = new List<string>();
		actTables = chall.NumOfTables;
		AvailableTables(chall.NumOfTables);
		if(chall.restMode == 1.0f) {
			FullRestaurant();
		}

		else if(chall.restMode == 2.0f) {
			BlackoutDay();
		}
		else if(chall.RestMode == 4.0f) {
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData("CustomerSpecialGossiper");
			Debug.Log(test.Script);
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			cus.GetComponent<CustomerSpecialGossiper>().init(chall.GossiperMode);
		}

		if(chall.SpecialDecoMode == 1) {
			PlayArea.Instance.cantLeave = true;
		}

		KitchenManager.Instance.Init(chall.KitchenTimerMod);
		string[] temp = DataLoaderChallengeMenuSet.GetData(chall.ChallengeMenuSet).ChallengeMenuSet;
		List<string> menuList = new List<string>();

		for(int i = 0; i< temp.Length; i++) {
			menuList.Add(temp[i]);
		}

        FoodManager.Instance.GenerateMenu(menuList);
		customerTimerDiffMod = chall.CustomerTimerMod;
		dayEarnedCash = 0;
		dayCashRevenue = 0;
		dayTime = chall.DayLengthMod;
		dayTimeLeft = dayTime;
		temp = DataLoaderCustomerSet.GetData(chall.CustomerSet).CustomerSet;
		Debug.Log(temp[0]);
		for(int i = 0; i < temp.Length; i++) {
			currCusSet.Add(temp[i]);
		}
		StartCoroutine("SpawnCustomer");
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer() {
		yield return 0;
		yield return new WaitForSeconds(customerSpawnTimer);

			if(!dayOver && lineCount < 8 && interval < currCusSet.Count) {
			
			doorController.OpenAndCloseDoor();

			if(isTutorial) {
				ImmutableDataCustomer test;
				test = DataLoaderCustomer.GetData("CustomerTutorial");
				GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				cus.GetComponent<Customer>().Init(customerNumber, eventData);
				cus.GetComponent<Customer>().behavFlow = test.BehavFlow;
				customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
				customerNumber++;
				challengeAI.AddCustomer();
			}
			else {
				ImmutableDataCustomer customerData;
				customerSpawnTimer = chall.CustSpawnTime;


				customerData = DataLoaderCustomer.GetData(currCusSet[interval]);

				// Track in analytics
				AnalyticsManager.Instance.TrackCustomerSpawned(customerData.ID);
				GameObject customerPrefab = Resources.Load(customerData.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				customerNumber++;
				cus.GetComponent<Customer>().behavFlow = customerData.BehavFlow;
				cus.GetComponent<Customer>().Init(customerNumber, chall);

				cus.GetComponent<Customer>().UpdateSatisfaction(chall.StartingHearts - 3);
				customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
				challengeAI.AddCustomer();
				interval++;
				customerNumber++;
				StartCoroutine("SpawnCustomer");
			}

		}
		else {
			// Call self to loop
			StartCoroutine("SpawnCustomer");
		}
	}
	

	public override void StartDay() {
		dayOver = false;
		RunSetUp();
		restaurantUI.StartDay();
	}

	/// <summary>
	/// Logic to calculate customer leaving because of a change in satisfaction, this includes angry/happy leaving
	/// </summary>
	/// <param name="isModifiesDifficulty">False for tutorials and challenges</param>
	public override void CustomerLeftSatisfaction(Customer customerData, bool isModifiesDifficulty, int VIPMultiplier = 1) {

		if(customerHash.ContainsKey(customerData.customerID)) {
			int satisfaction = customerData.satisfaction;
			int priceMultiplier;
			

			// Track analytics based on happy or angry leaving
			if(satisfaction > 0) { 
				AnalyticsManager.Instance.CustomerLeaveHappyChallenge(satisfaction, chall.ID);
				priceMultiplier = customerData.priceMultiplier * VIPMultiplier;

			}
			else {
				AnalyticsManager.Instance.CustomerLeaveAngryChallenge(customerData.type, customerData.state, chall.ID);

				priceMultiplier = 1;
			}

			// NOTE: Make sure not to track difficulty here
			UpdateCash(challengeAI.CalculateBill(satisfaction, priceMultiplier), customerData.transform.position);
			customerHash.Remove(customerData.customerID);
			CheckForGameOver();
		}
		else {
			Debug.LogError("Invalid CustomerLeftSatisfaction call on " + customerData.customerID);
		}
	}

	/// <summary>
	/// Logic to calculate customer leaving because of allergies (Go to hospital AND Saved)
	/// </summary>
	/// <param name="deltaCoins">Charge for using medic</param>
	/// <param name="isModifiesDifficulty">False for tutorials and challenges</param>
	public override void CustomerLeftFlatCharge(Customer customerData, int deltaCoins, bool isModifiesDifficulty) {
		if(customerHash.ContainsKey(customerData.customerID)) {
			// Track analytics leaving state though not really angry	
			AnalyticsManager.Instance.CustomerLeaveAngryChallenge(customerData.type, customerData.state, chall.ID);
			UpdateCash(challengeAI.CalculateBill(0, 1), customerData.transform.position);
			customerHash.Remove(customerData.customerID);
			CheckForGameOver();
		}
		else {
			Debug.LogError("Invalid CustomerLeftAllergy call on " + customerData.customerID);
		}
	}

	protected override void CheckForGameOver() {
		if(dayOver) {
			if(customerHash.Count == 0) {

				if(DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge == "ChallengeTut2") {
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 4 customers");
				}

			    if(isTutorial) {
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 2 guided customers");
					DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "ChallengeTut2";
					isTutorial = false;
					DataManager.Instance.GameData.RestaurantEvent.CustomerList.Add("CustomerRegular");
					StopCoroutine("SpawnCustomer");
					interval = 0;
					StartDay();
				}
				else {
					DataManager.Instance.GameData.DayTracker.ChallengesPlayed++;
					DataManager.Instance.ChallengesInSession++;

					AnalyticsManager.Instance.EndChallengeReport(challengeAI.ScoreIt(), DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge, challengeAI.MissingCustomers, challengeAI.AvgSatisfaction(), savedCustomers, attempted, inspectionButtonClicked);

					AnalyticsManager.Instance.EndGameUsageReport(playAreaUses, vipUses, microwaveUses);

					// Show day complete UI
					restaurantUI.DayComplete(challengeAI.MissingCustomers, dayEarnedCash, 0, dayCashRevenue);
					//restaurantUI.ChallengeComplete(challengeAI.Score,dayEarnedCash, challengeAI.MissingCustomers);
					DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
					// Save game data
					DataManager.Instance.SaveGameData();
				}
			}
		}
	}

	public void BlackoutDay() {
		blackoutImg.SetActive(true);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i++) {
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(false);
		}
		StartCoroutine(LightsOn());
	}

	private IEnumerator LightsOn() {
		yield return new WaitForSeconds(5.0f);
		blackoutImg.SetActive(false);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i++) {
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(true);
		}
		StartCoroutine(BlackoutAgain());
	}

	private IEnumerator BlackoutAgain() {
		yield return new WaitForSeconds(10.0f);
		blackoutImg.SetActive(true);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i++) {
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(false);
		}
		StartCoroutine(LightsOn());
	}

	public override void Blackout() {
		blackoutImg.SetActive(true);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i++) {
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(false);
		}
		StopCoroutine("SpawnCustomer");
		StartCoroutine(LightsOut());
	}

	private IEnumerator LightsOut() {
		yield return new WaitForSeconds(5.0f);
		blackoutImg.SetActive(false);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i++) {
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(true);
		}
		StartCoroutine("SpawnCustomer");
	}

	private void FullRestaurant() {
		StartCoroutine("WaitASec");
		//cus.GetComponent<Customer>().JumpToTable(i);
	}

	IEnumerator WaitASec() {
		yield return (0);
		for(int i = 0; i < 4; i++) {
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData(currCusSet[0]);
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			cus.GetComponent<Customer>().behavFlow = test.BehavFlow;
			cus.GetComponent<Customer>().tableNum = i;
            cus.GetComponent<Customer>().Init(customerNumber, chall);
			customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
			customerNumber++;
			challengeAI.AddCustomer();
			//sitting down
			cus.transform.SetParent(RestaurantManager.Instance.GetTable(cus.GetComponent<Customer>().tableNum).Seat);
			cus.transform.localPosition = Vector3.zero;
			// begin reading menu
			cus.GetComponent<Customer>().customerAnim.SetReadingMenu();

			// TODO-SOUND Reading menu here
			cus.GetComponent<Customer>().StopCoroutine("SatisactionTimer");

			// Table connection setup
			cus.gameObject.GetComponentInParent<Table>().currentCustomerID = cus.GetComponent<Customer>().customerID;
			cus.GetComponent<BoxCollider>().enabled = false;
			RestaurantManager.Instance.lineController.FillInLine();
			var type = Type.GetType(DataLoaderBehav.GetData(cus.GetComponent<Customer>().behavFlow).Behav[1]);
			Behav read = (Behav)Activator.CreateInstance(type);
			read.self = cus.GetComponent<Customer>();
			read.Act();
			//BehavReadingMenu read = new BehavReadingMenu(self);
			cus.GetComponent<Customer>().currBehav = read;
			read = null;
			GetTable(i).inUse = true;
		}
	}

	void Update() {
		if(!isPaused && dayOver == false) {
			dayTimeLeft -= Time.deltaTime;
			restaurantUI.UpdateClock(dayTime, dayTimeLeft);
			if(dayTimeLeft < 0) {
				dayOver = true;
				restaurantUI.FinishClock();
			}
		}
	}
	public void AvailableTables(int tabs) {
		for (int i = 3; i > tabs-1; i--) {
			Debug.Log(tableList.Count);
			Destroy(tableList[i]);
			tableList.RemoveAt(i);
		}
	}

	public void SpecialDecoSetup(int rule) {
		switch(rule) {
			case 1:
				PlayArea.Instance.cantLeave = true;
				break;
		}
	}
	public void SpawnSecondTut() {
		ImmutableDataCustomer test;
		test = DataLoaderCustomer.GetData("CustomerTutorial");
		GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
		GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
		cus.GetComponent<CustomerTutorial>().isAllergy = true;
		cus.GetComponent<Customer>().behavFlow = test.BehavFlow;
		cus.GetComponent<Customer>().Init(customerNumber,chall);
		customerHash.Add(cus.GetComponent<Customer>().customerID, cus);

		customerNumber++;
		challengeAI.AddCustomer();
	}

	public ChallengeReward RewardScore() {
		if(challengeAI.Score >= chall.GoldBreakPoint) {
			DataManager.Instance.GameData.chall.challengeProgress[chall.ID] = ChallengeReward.Gold;
			return ChallengeReward.Gold;
		}

		else if(challengeAI.Score >= chall.SilverBreakPoint) {
			if(DataManager.Instance.GameData.chall.challengeProgress[chall.ID] != ChallengeReward.Gold) {
				DataManager.Instance.GameData.chall.challengeProgress[chall.ID] = ChallengeReward.Silver;
			}
			return ChallengeReward.Silver;
		}
		else if (challengeAI.Score >= chall.BronzeBreakPoint) {
			if(DataManager.Instance.GameData.chall.challengeProgress[chall.ID] != ChallengeReward.Gold || DataManager.instance.GameData.chall.challengeProgress[chall.ID] != ChallengeReward.Silver) {
				DataManager.Instance.GameData.chall.challengeProgress[chall.ID] = ChallengeReward.Bronze;
			}
			return ChallengeReward.Bronze;
		}
		else {
			return ChallengeReward.Stone;
		}
	}
}
