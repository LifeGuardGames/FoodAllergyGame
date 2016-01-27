using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;
using System;

public class RestaurantManagerArcade : RestaurantManager {


	public override void Init() {
		eventData = DataLoaderEvents.GetData(DataManager.instance.GetEvent());
		sickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();

		// Only generate debug menu if menulist is not populated TODO removemenu set now, fix
		if(DataManager.Instance.IsDebug && FoodManager.Instance.MenuList == null) {
			FoodManager.Instance.GenerateMenu(DataLoaderRemoveMenuSet.GetData("RemoveMenuSetT1").RemoveMenuSet.ToList());
		}

		if(DataManager.Instance.GetEvent() == "EventTPlayArea" || DataManager.Instance.GetEvent() == "EventTFlyThru") {
			//TODO remove this and active tut screen
			StartDay();
		}

		else {
			StartDay();
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
		if(eventData.ID == "EventT1"){
			isTutorial = true;
			//customerSpawnTimer = customerTimer / satisfactionAI.DifficultyLevel + 1;
		}
		else if (eventData.ID == "EventTPlayArea"){
			dayTime = eventData.DayLengthMod;
			dayTimeLeft = dayTime;
			RunPlayAreaTut();
		}
		else{
			dayTime = eventData.DayLengthMod;
			dayTimeLeft = dayTime;

		}
		StartCoroutine(SpawnCustomer());
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer() {
		yield return 0;
		yield return new WaitForSeconds(customerSpawnTimer);

		if(isTutorial) {
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData("Customer10");
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			cus.GetComponent<Customer>().behavFlow = test.BehavFlow;
			customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
			customerNumber++;
			satisfactionAI.AddCustomer();
		}
		else {
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
				if(eventData.ID == "EventTPlayArea") {
					customerData = DataLoaderCustomer.GetData("Customer11");
					if(!DataManager.Instance.IsDebug) {
						//	DataManager.Instance.GameData.Decoration.DecoTutQueue.RemoveAt(0);

					}
				}
				else if(eventData.ID == "EventTVIP") {
					customerData = DataLoaderCustomer.GetData("Customer12");
				}
				else {

					customerData = DataLoaderCustomer.GetData(DataManager.Instance.GameData.RestaurantEvent.CustomerList[rand]);

					// Track in analytics
					AnalyticsManager.Instance.TrackCustomerSpawned(customerData.ID);
				}
				GameObject customerPrefab = Resources.Load(customerData.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				customerNumber++;
				cus.GetComponent<Customer>().behavFlow = customerData.BehavFlow;
				cus.GetComponent<Customer>().Init(customerNumber, eventData);
				cus.GetComponent<Customer>().behavFlow = customerData.BehavFlow;
				customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
				satisfactionAI.AddCustomer();
				StartCoroutine(SpawnCustomer());
			}
			else {
				// Call self to loop
				StartCoroutine(SpawnCustomer());
			}
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
			// Track analytics leaving state though not really angry
			AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);

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

	private void RunPlayAreaTut() {
		StartCoroutine("WaitASec");
		//cus.GetComponent<Customer>().JumpToTable(i);
	}

	IEnumerator WaitASec() {
		yield return (0);
		for(int i = 0; i < 4; i++) {
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData(currCusSet[1]);
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
			customerNumber++;
			satisfactionAI.AddCustomer();
			cus.GetComponent<Customer>().tableNum = i;
			cus.GetComponent<Behav>().Reason();
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
