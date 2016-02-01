using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;
using System;

public class RestaurantManagerChallenge : RestaurantManager{

	ImmutableDataChallenge chall;
	int interval = 0;
	

	public override void Init() {
		sickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();
		RunSetUp();
		StartDay();
	}


	private void RunSetUp() {
		chall = DataLoaderChallenge.GetData(DataManager.Instance.GetChallenge());

		if(chall.restMode == 1.0f) {
			FullRestaurant();
		}

		else if(chall.restMode == 2.0f) {
			BlackoutDay();
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

		StartCoroutine(SpawnCustomer());
		
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer() {
		yield return 0;
		yield return new WaitForSeconds(customerSpawnTimer);

			if(!dayOver && lineCount < 8 && interval < currCusSet.Count) {

				doorController.OpenAndCloseDoor();

				ImmutableDataCustomer customerData;
				customerSpawnTimer = chall.CustSpawnTime;
				
		
				customerData = DataLoaderCustomer.GetData(DataManager.Instance.GameData.RestaurantEvent.CustomerList[interval]);

				// Track in analytics
				AnalyticsManager.Instance.TrackCustomerSpawned(customerData.ID);
			
				GameObject customerPrefab = Resources.Load(customerData.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				customerNumber++;
				cus.GetComponent<Customer>().Init(customerNumber, chall);
				cus.GetComponent<Customer>().UpdateSatisfaction(chall.StartingHearts);
				customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
				satisfactionAI.AddCustomer();
				interval++;
				StartCoroutine(SpawnCustomer());

		}
		else {
			// Call self to loop
			StartCoroutine(SpawnCustomer());
		}
	}
	

	public override void StartDay() {
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
			float time;

			// Track analytics based on happy or angry leaving
			if(satisfaction > 0) {
				// TODO: NEW SET OF ANALYTICS FOR CHALLENGES
				/*
				AnalyticsManager.Instance.CustomerLeaveHappy(satisfaction);
				*/

				priceMultiplier = customerData.priceMultiplier * VIPMultiplier;
				time = Time.time - customerData.spawnTime;
			}
			else {
				// TODO: NEW SET OF ANALYTICS FOR CHALLENGES
				/*
				AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);
				*/

				priceMultiplier = 1;
				time = RestaurantManager.customerLeaveModifierTime;
			}

			// NOTE: Make sure not to track difficulty here
			UpdateCash(satisfactionAI.CalculateBill(satisfaction, priceMultiplier, time, false), customerData.transform.position);
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
			// TODO: NEW SET OF ANALYTICS FOR CHALLENGES
			/*
			AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);
			*/

			UpdateCash(satisfactionAI.CalculateBill(0, 1, RestaurantManager.customerLeaveModifierTime, isModifiesDifficulty), customerData.transform.position);
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
				// TODO make challenge versions of them?
//				DataManager.Instance.GameData.DayTracker.DaysPlayed++;
//				DataManager.Instance.DaysInSession++;
//
//				AnalyticsManager.Instance.EndGameDayReport(CashManager.Instance.TotalCash,
//					DataManager.Instance.GameData.RestaurantEvent.CurrentEvent, satisfactionAI.MissingCustomers, satisfactionAI.AvgSatisfaction(),
//					DayEarnedCash, Medic.Instance.MedicCost, savedCustomers, attempted, inspectionButtonClicked);
//
//				AnalyticsManager.Instance.EndGameUsageReport(playAreaUses, vipUses, microwaveUses);

				// Show day complete UI
				//restaurantUI.DayComplete(satisfactionAI.MissingCustomers, dayEarnedCash, Medic.Instance.MedicCost, dayNetCash);

				// Save game data
//				DataManager.Instance.SaveGameData();
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
		StopCoroutine(SpawnCustomer());
		StartCoroutine(LightsOut());
	}

	private IEnumerator LightsOut() {
		yield return new WaitForSeconds(5.0f);
		blackoutImg.SetActive(false);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i++) {
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(true);
		}
		StartCoroutine(SpawnCustomer());
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
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
			customerNumber++;
			satisfactionAI.AddCustomer();
			//cus.GetComponent<Customer>().JumpToTable(i);
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
			Destroy(tableList[i]);
		}
	}

	public void SpecialDecoSetup(int rule) {
		switch(rule) {
			case 1:
				PlayArea.Instance.cantLeave = true;
				break;
		}
	}
}
