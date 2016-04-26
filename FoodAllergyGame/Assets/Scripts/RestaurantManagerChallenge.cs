using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RestaurantManagerChallenge : RestaurantManager{
	// our satisfaction ai 
	private ChallengeAI challengeAI;
	public PlayAreaLoader play;
	public VIPLoader vip;
	public FlyThruLoader fly;
	public ChallengeScoreController scoreBoard;
	public GameObject skipButton;
	public ImmutableDataChallenge chall;
	int interval = 0;

	public override void Init() {
		sickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		challengeAI = new ChallengeAI();
	}
	
	private void RunSetUp() {
		chall = DataLoaderChallenge.GetData(DataManager.Instance.GetChallenge());
		if(chall.ChallengeType != ChallengeTypes.Tutorial) {
			scoreBoard.gameObject.SetActive(true);
			scoreBoard.UpDateScore(0);
		}
		if(chall.PlayArea != "0") {
			play.LoadDeco(DataLoaderDecoItem.GetData(chall.PlayArea));
		}
		if(chall.VipTable != "0") {
			vip.LoadDeco(DataLoaderDecoItem.GetData(chall.VipTable));
		}
		if(chall.FlyThru != "0") {
			fly.LoadDeco(DataLoaderDecoItem.GetData(chall.FlyThru));
		}
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
			if(isTutorial) {
				skipButton.SetActive(true);
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
				//stops the user from placing the VIP tut customer at a regular table
				if(DataManager.Instance.GetChallenge() == "TutDecoVIP") {
					foreach(GameObject tab in TableList) {
						if (tab.GetComponent<Table>().tableType != Table.TableType.VIP) {
							tab.GetComponent<Table>().inUse = true;
						}
					}
				}
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
			scoreBoard.UpDateScore(challengeAI.ScoreIt());
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
			if(deltaCoins == 0) {
				UpdateCash(challengeAI.CalculateBill(0, 1), customerData.transform.position);
			}
			else {
				challengeAI.CalculateBill(0, 1);
				challengeAI.AddNegativeCash(deltaCoins);
			}
			customerHash.Remove(customerData.customerID);
			CheckForGameOver();
		}
		else {
			Debug.LogError("Invalid CustomerLeftAllergy call on " + customerData.customerID);
		}
	}

	public override void CheckTablesForGameOver() {
		if(actTables == 0) {
			// no tables so force a end of day
			dayOver = true;
			foreach(KeyValuePair<string, GameObject> val in customerHash) {
				Destroy(val.Value);
			}
			customerHash.Clear();
			CheckForGameOver();
		}
	}

	protected override void CheckForGameOver() {
		if(dayOver) {
			if(customerHash.Count == 0) {
				pauseUI.isActive = false;
				if(DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge == "ChallengeTut2") {
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 4 customers");
				}

			    if(isTutorial) {
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 2 guided customers");
					DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					DataManager.Instance.SaveGameData();
					DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "ChallengeTut2";
					isTutorial = false;
					DataManager.Instance.GameData.RestaurantEvent.CustomerList.Add("CustomerRegular");
					StopCoroutine("SpawnCustomer");
					interval = 0;
					customerSpawnTimer = 0;
					TableList[0].GetComponent<Table>().inUse = false;
					TableList[1].GetComponent<Table>().inUse = false;
					pauseUI.isActive = true;
					restaurantUI.ResetDoor();
					StartDay();
				}
				else {
					if(TierManager.Instance.CurrentTier == 0) {
						DataManager.Instance.GameData.Cash.TotalCash = 850;
					}
					DataManager.Instance.GameData.DayTracker.ChallengesPlayed++;
					DataManager.Instance.ChallengesInSession++;

					AnalyticsManager.Instance.EndChallengeReport(challengeAI.ScoreIt(), DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge, challengeAI.MissingCustomers, challengeAI.AvgSatisfaction(), savedCustomers, attempted, inspectionButtonClicked);
					AnalyticsManager.Instance.EndGameUsageReport(playAreaUses, vipUses, microwaveUses);

					// Show day complete UI
					if(chall.ChallengeType == ChallengeTypes.Tutorial) {
						restaurantUI.DayComplete(challengeAI.MissingCustomers, dayEarnedCash, 0, dayCashRevenue);
					}
					else {
						restaurantUI.ChallengeComplete(challengeAI.Score, dayEarnedCash, challengeAI.NegativeCash);
					}
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
			if(chall.ID == "TutDecoPlayArea") {
				test = DataLoaderCustomer.GetData("CustomerRegular");
			}
			else {
				test = DataLoaderCustomer.GetData(currCusSet[0]);
			}
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			Customer customerScript = cus.GetComponent<Customer>();

			customerScript.behavFlow = test.BehavFlow;
			customerScript.tableNum = i;
			customerScript.Init(customerNumber, chall);
			customerHash.Add(customerScript.customerID, cus);
			customerNumber++;
			challengeAI.AddCustomer();

			//sitting down
			cus.transform.SetParent(GetTable(customerScript.tableNum).Seat);
			customerScript.SetBaseSortingOrder(GetTable(customerScript.tableNum).BaseSortingOrder);
            cus.transform.localPosition = Vector3.zero;

			// begin reading menu
			customerScript.customerAnim.SetReadingMenu();

			// TODO-SOUND Reading menu here
			customerScript.StopCoroutine("SatisactionTimer");

			// Table connection setup
			cus.gameObject.GetComponentInParent<Table>().currentCustomerID = customerScript.customerID;
			cus.GetComponent<BoxCollider>().enabled = false;
			lineController.FillInLine();
			var type = Type.GetType(DataLoaderBehav.GetData(customerScript.behavFlow).Behav[1]);
			Behav read = (Behav)Activator.CreateInstance(type);
			read.self = customerScript;
			read.Act();
			//BehavReadingMenu read = new BehavReadingMenu(self);
			customerScript.currBehav = read;
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
		while(tableList.Count > chall.NumOfTables) {
			Destroy(tableList[0]);
			tableList.RemoveAt(0);
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
		challengeAI.ScoreIt();
		if(challengeAI.Score >= chall.BronzeBreakPoint) {
			if(chall.IsBossChallenge) {
				DataManager.Instance.GameData.Challenge.BossConquored(chall.ID);
			}
		}
		if(challengeAI.Score >= chall.GoldBreakPoint) {
			DataManager.Instance.GameData.Challenge.ChallengeProgress[chall.ID] = ChallengeReward.Gold;
			return ChallengeReward.Gold;
		}

		else if(challengeAI.Score >= chall.SilverBreakPoint) {
			if(DataManager.Instance.GameData.Challenge.ChallengeProgress[chall.ID] != ChallengeReward.Gold) {
				DataManager.Instance.GameData.Challenge.ChallengeProgress[chall.ID] = ChallengeReward.Silver;
			}
			return ChallengeReward.Silver;
		}
		else if (challengeAI.Score >= chall.BronzeBreakPoint) {
			if(DataManager.Instance.GameData.Challenge.ChallengeProgress[chall.ID] != ChallengeReward.Gold || DataManager.instance.GameData.Challenge.ChallengeProgress[chall.ID] != ChallengeReward.Silver) {
				DataManager.Instance.GameData.Challenge.ChallengeProgress[chall.ID] = ChallengeReward.Bronze;
			}
			return ChallengeReward.Bronze;
		}
		else {
			return ChallengeReward.Stone;
		}
	}
	public int GetScore() {
		return challengeAI.Score;
	}
	
}
