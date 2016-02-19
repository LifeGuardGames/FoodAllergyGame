﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;
using System;

public class RestaurantManagerArcade : RestaurantManager {

	// our satisfaction ai 
	private SatisfactionAI satisfactionAI;
	public override void Init() {
		eventData = DataLoaderEvents.GetData(DataManager.instance.GetEvent());
		sickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();

		// Only generate debug menu if menulist is not populated TODO removemenu set now, fix
		if(DataManager.Instance.IsDebug && FoodManager.Instance.MenuList == null) {
			FoodManager.Instance.GenerateMenu(DataLoaderRemoveMenuSet.GetData("RemoveMenuSetT1").RemoveMenuSet.ToList());
		}
	}

	public override void StartDay() {
		this.eventData = DataLoaderEvents.GetData(DataManager.instance.GetEvent());
		customerTimerDiffMod = DataManager.Instance.GameData.DayTracker.AvgDifficulty;
		string currSet = eventData.CustomerSet;
		currCusSet = new List<string>(DataLoaderCustomerSet.GetData(currSet).CustomerSet);
		KitchenManager.Instance.Init(this.eventData.KitchenTimerMod);
		dayEarnedCash = 0;
		dayCashRevenue = 0;
		restaurantUI.StartDay();
		//Debug.Log("Starting Day - Event:" +  eventData.ID + ", Customer Set:" + currSet);

		dayTime = eventData.DayLengthMod;
		dayTimeLeft = dayTime;

		StartCoroutine(SpawnCustomer());
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer() {
		yield return 0;
		yield return new WaitForSeconds(customerSpawnTimer);

			int rand;
		if(!dayOver && lineCount < 8) {

			doorController.OpenAndCloseDoor();

			ImmutableDataCustomer customerData;
			if(DataManager.Instance.GameData.DayTracker.AvgDifficulty == 15.0f) {
				customerSpawnTimer = 6.0f;
			}
			else if(IsTableAvilable()) {
				customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty * 0.27f;
			}
			else {
				customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty * 0.4f;
			}

			if(customerSpawnTimer < 3.0f) {
				customerSpawnTimer = 3.0f;
			}

			//Debug.Log(customerSpawnTimer);
			rand = UnityEngine.Random.Range(0, DataManager.Instance.GameData.RestaurantEvent.CustomerList.Count);
			customerData = DataLoaderCustomer.GetData(DataManager.Instance.GameData.RestaurantEvent.CustomerList[rand]);

			// Track in analytics
			AnalyticsManager.Instance.TrackCustomerSpawned(customerData.ID);

			GameObject customerPrefab = Resources.Load(customerData.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			customerNumber++;
			cus.GetComponent<Customer>().behavFlow = customerData.BehavFlow;
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
			satisfactionAI.AddCustomer();
			StartCoroutine(SpawnCustomer());
		}
		else {
			// Call self to loop
			StartCoroutine(SpawnCustomer());
		}
	}

	/// <summary>
	/// Logic to calculate customer leaving because of a change in satisfaction
	/// </summary>
	/// <param name="isModifiesDifficulty">False for tutorials and challenges</param>
	public override void CustomerLeftSatisfaction(Customer customerData, bool isModifiesDifficulty, int VIPMultiplier = 1) {

		if(customerHash.ContainsKey(customerData.customerID)) {
			int satisfaction = customerData.satisfaction;
			int priceMultiplier;
			float time;

			// Track analytics based on happy or angry leaving
			if(satisfaction > 0 && customerData.state != CustomerStates.Eaten) {
				AnalyticsManager.Instance.CustomerLeaveHappy(satisfaction);
				priceMultiplier = customerData.priceMultiplier * VIPMultiplier;
				time = Time.time - customerData.spawnTime;
            }
			else {
				AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);
				priceMultiplier = 1;
				time = customerLeaveModifierTime;
			}

			UpdateCash(satisfactionAI.CalculateBill(satisfaction, priceMultiplier, time, isModifiesDifficulty), customerData.transform.position);
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
			// Add to restaurant expense
			Medic.Instance.BillRestaurant(deltaCoins);

			// Track analytics leaving state though not really angry
			AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);

			// Use predefined values here for negative balancing, only counts if isModifiesDifficulty == true, returns 0
			UpdateCash(satisfactionAI.CalculateBill(0, 1, customerLeaveModifierTime, isModifiesDifficulty), customerData.transform.position);
			customerHash.Remove(customerData.customerID);
			CheckForGameOver();
		}
		else {
			Debug.LogError("Invalid CustomerLeftAllergy call on " + customerData.customerID);
		}
	}
	public override void checkTablesForGameOver() {
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
				DataManager.Instance.GameData.DayTracker.DaysPlayed++;
				DataManager.Instance.DaysInSession++;
				
					DataManager.Instance.GameData.DayTracker.AvgDifficulty = ((DataManager.Instance.GameData.DayTracker.AvgDifficulty + satisfactionAI.DifficultyLevel) / 2);
					// Save data here
					int dayNetCash = dayEarnedCash + Medic.Instance.MedicCost;
					CashManager.Instance.RestaurantEndCashUpdate(dayNetCash, dayCashRevenue);

					// Unlock new event generation for StartManager
					DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;

					
//					else if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT3") {
//						DataManager.Instance.GameData.Tutorial.IsTutorial3Done = true;
//						AnalyticsManager.Instance.TutorialFunnel("Menu tut day complete");
//						CashManager.Instance.TutorialOverrideTotalCash(850);
//					}

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

	// Called from PauseUIController
	public override void QuitGame() {
		Time.timeScale = 1.0f;  // Remember to reset timescale!
		if(!dayOver) {
			IncompleteQuitAnalytics();
		}

		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
	}

	// Used in OnApplicationPaused in Restaurant and quit button
	public override void IncompleteQuitAnalytics() {
		AnalyticsManager.Instance.TrackGameDayInRestaurant(dayTimeLeft, TierManager.Instance.CurrentTier, DataManager.Instance.GameData.RestaurantEvent.CurrentEvent,
				satisfactionAI.DifficultyLevel, satisfactionAI.MissingCustomers, satisfactionAI.AvgSatisfaction(),
				DayEarnedCash, Medic.Instance.MedicCost);
	}

}
