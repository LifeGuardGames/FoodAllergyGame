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
		SickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();
		RunSetUp();
		StartDay();
	}


	private void RunSetUp() {
		chall = DataLoaderChallenge.GetData(DataManager.Instance.GetChallenge());
		if(chall.restMode == 1.0) {
			FullRestaurant();
		}
		KitchenManager.Instance.Init(chall.KitchenTimerMod);
		string[] temp = DataLoaderMenuSet.GetData(chall.MenuSet).MenuSet;
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

	public override void CustomerLeft(Customer customerData, bool isLeavingHappy, int satisfaction, int priceMultiplier, Vector3 customerPos, float time, bool earnedMoney) {
		if(customerHash.ContainsKey(customerData.customerID)) {

			// Track analytics based on happy or angry leaving
			if(isLeavingHappy) {
				AnalyticsManager.Instance.CustomerLeaveHappy(satisfaction);
			}
			else {
				AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);
			}

			UpdateCash(satisfactionAI.CalculateBill(satisfaction, priceMultiplier, customerPos, time, earnedMoney));
			customerHash.Remove(customerData.customerID);
			CheckForGameOver();
		}
		else {
			Debug.LogError("Invalid call on " + customerData.customerID);
		}
	}

	protected override void CheckForGameOver() {
		if(dayOver) {
			if(customerHash.Count == 0) {
				DataManager.Instance.GameData.DayTracker.DaysPlayed++;
				DataManager.Instance.DaysInSession++;
				if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT2") {
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 4 customers");
				}
				if(isTutorial) {
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 2 guided customers");
					DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT2";
					isTutorial = false;
					dayOver = false;
					StopCoroutine(SpawnCustomer());
					StartDay();
				}
				else {
					DataManager.Instance.GameData.DayTracker.AvgDifficulty = ((DataManager.Instance.GameData.DayTracker.AvgDifficulty + satisfactionAI.DifficultyLevel) / 2);
					// Save data here
					int dayNetCash = dayEarnedCash + Medic.Instance.MedicCost;
					CashManager.Instance.RestaurantEndCashUpdate(dayNetCash, dayCashRevenue);

					// Unlock new event generation for StartManager
					DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;

					// Set tutorial to done if applies
					if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1") {
						DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					}
					else if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT3") {
						DataManager.Instance.GameData.Tutorial.IsTutorial3Done = true;
						AnalyticsManager.Instance.TutorialFunnel("Menu tut day complete");
						CashManager.Instance.TutorialOverrideTotalCash(850);
					}

					AnalyticsManager.Instance.EndGameDayReport(CashManager.Instance.TotalCash,
						DataManager.Instance.GameData.RestaurantEvent.CurrentEvent, satisfactionAI.MissingCustomers, satisfactionAI.AvgSatisfaction(),
						DayEarnedCash, Medic.Instance.MedicCost, savedCustomers, attempted, inspectionButtonClicked);

					AnalyticsManager.Instance.EndGameUsageReport(playAreaUses, vipUses, microwaveUses);

					// Show day complete UI
					restaurantUI.DayComplete(satisfactionAI.MissingCustomers, dayEarnedCash, Medic.Instance.MedicCost, dayNetCash);

					// Save game data
					DataManager.Instance.SaveGameData();
				}
			}
		}
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
			cus.GetComponent<Customer>().JumpToTable(i);
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
}
