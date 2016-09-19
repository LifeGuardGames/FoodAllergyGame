using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RestaurantManagerArcade : RestaurantManager {

	public int flowMod;
	public List<string> flowList;
	public Dictionary<string, object> customerList;
	public PositionTweenToggle BlackoutTut;
	public PositionTweenToggle GossiperTut;

	// our satisfaction ai
	private SatisfactionAI satisfactionAI;
	public override void Init() {
		customerList = new Dictionary<string, object>();
		eventData = DataLoaderEvents.GetData(DataManager.instance.GetEvent());
		sickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();

		// Only generate debug menu if menulist is not populated TODO removemenu set now, fix
		if(DataManager.Instance.IsDebug && FoodManager.Instance.MenuList == null) {
			FoodManager.Instance.GenerateMenu(DataLoaderRemoveMenuSet.GetData("RemoveMenuSetT1").RemoveMenuSet.ToList());
		}
		MiddlePhase();
	}

	private void RunSetUp() {
		DifficultyAI diffAi = new DifficultyAI();
		diffAi.Init(DataManager.Instance.GameData.DayTracker.AvgDifficulty, eventData);
			if(eventData.RestMode == 1.0f) {
				FullRestaurant();
			}

			else if(eventData.RestMode == 2.0f) {
				BlackoutDay();
			}
			else if(eventData.RestMode == 4.0f) {
				ImmutableDataCustomer test;
				test = DataLoaderCustomer.GetData("CustomerSpecialGossiper");
				GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				cus.GetComponent<CustomerSpecialGossiper>().init(4);
			}
	}

	public override void StartDay() {
		AnalyticsManager.Instance.SuperProperties.Add("Event", DataManager.Instance.GetEvent());
		this.eventData = DataLoaderEvents.GetData(DataManager.instance.GetEvent());
		customerTimerDiffMod = DataManager.Instance.GameData.DayTracker.AvgDifficulty;
		string currSet = eventData.CustomerSet;
		currCusSet = new List<string>(DataLoaderCustomerSet.GetData(currSet).CustomerSet);
		KitchenManager.Instance.Init(this.eventData.KitchenTimerMod);
		dayEarnedCash = 0;
		dayCashRevenue = 0;
		restaurantUI.StartDay();
		//Debug.Log("Starting Day - Event:" +  eventData.ID + ", Customer Set:" + currSet);
		actTables = 4;
		dayTime = eventData.DayLengthMod;
		//Sanity Check to ensure restauranrt doesn't start closed
		if(dayTime < 10.0f) {
			dayTime = 72.0f;
		}
		dayTimeLeft = dayTime;
		flowList = new List<string>();
		string [] temp = DataLoaderCustomerFlow.GetData(eventData.FlowList).FlowList;
		for(int i = 0; i < temp.Length; i++) {
			flowList.Add(temp[i]);
		}
		RunSetUp();
		if(eventData.RestMode == 2.0f && !DataManager.Instance.GameData.Tutorial.IsBlackOutTutDone) {
			BlackoutTut.Show();
		}
		else if(eventData.RestMode == 4.0f && !DataManager.Instance.GameData.Tutorial.IsGossiperTutDone) {
			GossiperTut.Show();
		}
		else {
			StartCoroutine("NextWave");
			StartCoroutine(SpawnCustomer());
		}
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer() {
		yield return 0;
		yield return new WaitForSeconds(customerSpawnTimer);

		int rand;
		if(!dayOver && lineCount < 8) {
			ImmutableDataCustomer customerData;
			if(DataManager.Instance.GameData.DayTracker.AvgDifficulty == 15.0f) {
				customerSpawnTimer = 6.0f;
			}
			else if(IsTableAvailable()) {
				customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty * 0.27f + flowMod;
			}
			else {
				customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty * 0.4f + flowMod;
			}

			if(customerSpawnTimer < 3.0f) {
				customerSpawnTimer = 3.0f;
			}

			//Debug.Log(customerSpawnTimer);
			rand = UnityEngine.Random.Range(0, DataManager.Instance.GameData.RestaurantEvent.CustomerList.Count);
			customerData = DataLoaderCustomer.GetData(DataManager.Instance.GameData.RestaurantEvent.CustomerList[rand]);

			GameObject customerPrefab = Resources.Load(customerData.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			customerNumber++;
			cus.GetComponent<Customer>().behavFlow = DataLoaderBehav.GetRandomBehavByType(cus.GetComponent<Customer>().type.ToString()).ID;
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			AddCustomer(cus.GetComponent<Customer>());
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
				AnalyticsManager.Instance.CustomerLeaveHappy(customerData.type,satisfaction);
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
				DataManager.Instance.GameData.DayTracker.DaysPlayed++;
				DataManager.Instance.GameData.DayTracker.GoalTimeLimit--;
				DataManager.Instance.DaysInSession++;
				
				DataManager.Instance.GameData.DayTracker.AvgDifficulty = ((DataManager.Instance.GameData.DayTracker.AvgDifficulty + satisfactionAI.DifficultyLevel) / 2);
				// Save data here
					int dayNetCash;
				if(checkBonus()) {
					dayNetCash = dayEarnedCash + Medic.Instance.MedicCost + 200;
				}
				else {
					dayNetCash = dayEarnedCash + Medic.Instance.MedicCost;
				}
                    CashManager.Instance.RestaurantEndCashUpdate(dayNetCash, dayCashRevenue);
				if(TierManager.Instance.CurrentTier == 0) {
					DataManager.Instance.GameData.Cash.TotalCash = 850;
				}
				// Unlock new event generation for StartManager
				DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;


				//					else if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT3") {
				//						DataManager.Instance.GameData.Tutorial.IsTutorial3Done = true;
				//						AnalyticsManager.Instance.TutorialFunnel("Menu tut day complete");
				//						CashManager.Instance.TutorialOverrideTotalCash(850);
				//					}
				//AnalyticsManager.Instance.TrackCustomerSpawned(DataManager.Instance.GetEvent(),customerList);
				AnalyticsManager.Instance.SuperProperties.Remove("Event");
			
					AnalyticsManager.Instance.EndGameDayReport(
						DataManager.Instance.GameData.RestaurantEvent.CurrentEvent, satisfactionAI.MissingCustomers, satisfactionAI.AvgSatisfaction(),
						DayEarnedCash, Medic.Instance.MedicCost, savedCustomers, attempted, inspectionButtonClicked);
				if(TierManager.Instance.CurrentTier > 2) {
					AnalyticsManager.Instance.VIPUsage(vipUses);
				}
				if(TierManager.Instance.CurrentTier > 4) {
					AnalyticsManager.Instance.PlayAreaUsage(playAreaUses);
				}
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
		if(GetCurrentCustomers().Count > 1) {
			List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
			for(int i = 0; i < currCustomers.Count; i++) {
				if(currCustomers[i] != null) {
					currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(true);
				}
			}
			StartCoroutine(SpawnCustomer());
		}
		CheckForGameOver();
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

	// Used in OnApplicationPaused in Restaurant and quit button
	public override void IncompleteQuitAnalytics() {
		AnalyticsManager.Instance.SuperProperties.Remove("Event");
		AnalyticsManager.Instance.TrackGameDayInRestaurantArcade(dayTimeLeft, TierManager.Instance.CurrentTier, DataManager.Instance.GameData.RestaurantEvent.CurrentEvent,
				satisfactionAI.DifficultyLevel, satisfactionAI.MissingCustomers, satisfactionAI.AvgSatisfaction(),
				DayEarnedCash, Medic.Instance.MedicCost, eventData.RestMode);
	}

	public bool checkBonus() {
		ImmutableDataBonusObjective temp = DataLoaderBonusObjective.GetData(DataManager.Instance.GetBonus());
		switch(temp.ObjType) {
			case "Cash":
				if(temp.Num < dayEarnedCash) {
					return true;
				}
				break;
			case "AllergyAttack":
				if(temp.Num >= savedCustomers) {
					return true;
				}
				break;
			case "Missed":
				if(temp.Num < satisfactionAI.MissingCustomers) {
					return true;
				}
				break;
			case "wheat":
				if(temp.Num < wheatServed) {
					return true;
				}
				break;
			case "dairy":
				if(temp.Num < dairyServed) {
					return true;
				}
				break;
			case "peanut":
				if(temp.Num < wheatServed) {
					return true;
				}
				break;
			case "VIP":
				if(temp.Num < vipUses) {
					return true;
				}
				break;
			case "PlayArea":
				if(temp.Num < playAreaUses) {
					return true;
				}
				break;
		}
		return false;
	}

	IEnumerator NextWave() {
		yield return new WaitForSeconds(dayTime / 4);
		switch(flowList[0]) {
			case "0":
				flowMod = 0;
				break;
			case "1":
				flowMod = 5;
				break;
			case "-1":
				flowMod = -3;
				break;
		}
		flowList.RemoveAt(0);
		if(!dayOver) { 
		StartCoroutine("NextWave");
		}
	}

	public void AddCustomer(Customer cus) {
		if(customerList.ContainsKey(cus.type.ToString())) {
			int temp = (int)customerList[cus.type.ToString()];
            temp++;
			customerList[cus.type.ToString()] = temp;
        }
		else {
			customerList.Add(cus.type.ToString(), 1);
		}
	}

	public void AvailableTables(int tabs) {
		actTables = tabs;
		while(tableList.Count > tabs) {
			Destroy(tableList[0]);
			tableList.RemoveAt(0);
		}
	}

	private void FullRestaurant() {
		StartCoroutine("WaitASec");
		//cus.GetComponent<Customer>().JumpToTable(i);
	}

	IEnumerator WaitASec() {
		yield return (0);
		for(int i = 0; i < 4; i++) {
			ImmutableDataCustomer test;
			
			test = DataLoaderCustomer.GetData("CustomerRegular");
		
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			Customer customerScript = cus.GetComponent<Customer>();

			customerScript.behavFlow = test.BehavFlow;
			customerScript.tableNum = i;
			customerScript.Init(customerNumber, eventData);
			customerHash.Add(customerScript.customerID, cus);
			customerNumber++;
			satisfactionAI.AddCustomer();

			//sitting down
			if(GetTable(customerScript.tableNum) != null) {
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

	public void BlackoutMoveAlong() {
		DataManager.Instance.GameData.Tutorial.IsBlackOutTutDone = true;
		BlackoutTut.Hide();
		StartCoroutine("NextWave");
		StartCoroutine(SpawnCustomer());
	}

	public void GossipMoveAlong() {
		DataManager.Instance.GameData.Tutorial.IsBlackOutTutDone = true;
		GossiperTut.Hide();
		StartCoroutine("NextWave");
		StartCoroutine(SpawnCustomer());
	}

}
