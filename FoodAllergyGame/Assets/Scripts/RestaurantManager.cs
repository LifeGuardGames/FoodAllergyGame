using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;

public class RestaurantManager : Singleton<RestaurantManager>{
	public bool isPaused;
	private float customerSpawnTimer;
	public float dayTime;					// Total amount of time in the day
	private float dayTimeLeft;				// Current amount of time left in the day
	public int lineCount = 0;
	private int customerNumber = 0;
	public bool dayOver = false;			// bool controlling customer spawning depending on the stage of the day
	public int actTables;
	public int medicUsed;
	private int dayEarnedCash;				// The cash that is earned for the day
	public int DayEarnedCash{
		get{ return dayEarnedCash; }
	}
	
	private int dayCashRevenue;				// The total positive cashed gained for the day
	//tracks customers via hashtable
	private Dictionary<string, GameObject> customerHash;
	// our satisfaction ai 
	private SatisfactionAI satisfactionAI;
	public List<GameObject> SickCustomers;

	private List<GameObject> tableList = new List<GameObject>();
	public List<GameObject> TableList{
		get{ return tableList; }
	}

	private TableFlyThru flyThruTable = null;		// Cached table position
	private bool isFlyThruTableCached = false;

	public RestaurantUIManager restaurantUI;
	private ImmutableDataEvents eventData;
	public LineController lineController;
	public RestaurantMenuUIController menuUIController;
	public DoorController doorController;

	public bool firstSickCustomer = false;
	public GameObject medicButton;
	public GameObject medicTutorial;
	public GameObject blackoutImg;
	private List<string> currCusSet;
	public bool isTutorial;
	public bool isVIPTut;
	public PauseUIController pauseUI;
	public float baseCustomerTimer;
	public float customerTimerDiffMod;

	#region Analytics
	public int inspectionButtonClicked;
	private int attempted;
	public int savedCustomers;

	private int playAreaUses;
	public int PlayAreaUses{
		set{ playAreaUses = value; }
		get{ return playAreaUses; }
	}

	private int vipUses;
	public int VIPUses{
		set{ vipUses = value; }
		get{ return vipUses; }
	}

	private int microwaveUses;
	public int MicrowaveUses{
		set{ microwaveUses = value; }
		get{ return microwaveUses; }
	}
	#endregion
	
	void Start(){
		SickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();

		// Only generate debug menu if menulist is not populated
		if(DataManager.Instance.IsDebug && FoodManager.Instance.MenuList == null) {
			FoodManager.Instance.GenerateMenu(DataLoaderMenuSet.GetData("MenuSetT1").MenuSet.ToList());
		}

		if(DataManager.Instance.GetEvent() == "EventTPlayArea"||DataManager.Instance.GetEvent() == "EventTFlyThru"){
			//TODO remove this and active tut screen
			StartDay(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));
		}
		else{
			StartDay(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));
		}
	}

	// Called at the start of the game day begins the day tracker coroutine 
	public void StartDay(ImmutableDataEvents eventData){
		customerTimerDiffMod = DataManager.Instance.GameData.DayTracker.AvgDifficulty;
		this.eventData = eventData;
		string currSet = eventData.CustomerSet;
		currCusSet = new List<string>(DataLoaderCustomerSet.GetData(currSet).CustomerSet);
		KitchenManager.Instance.Init(eventData.KitchenTimerMod);
		dayEarnedCash = 0;
		dayCashRevenue = 0;
		restaurantUI.StartDay();
		Debug.Log("Starting Day - Event:" +  eventData.ID + ", Customer Set:" + currSet);
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

	// When complete flips the dayOver bool once the bool is true customers will cease spawning and the game will be looking for the end point
	void Update(){
		if(!isPaused && dayOver == false){
			dayTimeLeft -= Time.deltaTime;
			restaurantUI.UpdateClock(dayTime, dayTimeLeft);
			if(dayTimeLeft < 0){
				dayOver = true;
				restaurantUI.FinishClock();
			}
		}
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer(){
		yield return 0;
		yield return new WaitForSeconds(customerSpawnTimer);

		if(isTutorial){
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData("Customer10");
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
			customerNumber++;
			satisfactionAI.AddCustomer();
		}
		else{
			int rand;		
			if(!dayOver && lineCount < 8){
				
				doorController.OpenAndCloseDoor();

				ImmutableDataCustomer customerData;
				if(DataManager.Instance.GameData.DayTracker.AvgDifficulty == 6.0f) {
					customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty;
				}
				else if(IsTableAvilable()) {
					customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty * 0.27f;
				}
				else {
					customerSpawnTimer = DataManager.Instance.GameData.DayTracker.AvgDifficulty * 0.4f;
				}
				
				if(customerSpawnTimer < 3){
					customerSpawnTimer = 3;
				}
				rand = UnityEngine.Random.Range(0, DataManager.Instance.GameData.RestaurantEvent.CustomerList.Count);
				if(eventData.ID == "EventTPlayArea"){
					customerData = DataLoaderCustomer.GetData("Customer11");
					if(!DataManager.Instance.IsDebug){
					//	DataManager.Instance.GameData.Decoration.DecoTutQueue.RemoveAt(0);

					}
				}
				else if (eventData.ID == "EventTVIP"){
					customerData = DataLoaderCustomer.GetData("Customer12");
				}
				else{
					Debug.Log(rand);
					Debug.Log(DataManager.Instance.GameData.RestaurantEvent.CustomerList.Count);
					customerData = DataLoaderCustomer.GetData(DataManager.Instance.GameData.RestaurantEvent.CustomerList[rand]);

					// Track in analytics
					AnalyticsManager.Instance.TrackCustomerSpawned(customerData.ID);
                }
				GameObject customerPrefab = Resources.Load(customerData.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				customerNumber++;
				cus.GetComponent<Customer>().Init(customerNumber, eventData);

				customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
				satisfactionAI.AddCustomer();
				StartCoroutine(SpawnCustomer());

			}
			else{
				// Call self to loop
				StartCoroutine(SpawnCustomer());
			}
		}
	}

	// Removes a customer from the dictionary
	// and then if the day is over checks to see if the dictionary is empty and if it is it ends the round
	public void CustomerLeft(Customer customerData, bool isLeavingHappy, int satisfaction, int priceMultiplier, Vector3 customerPos, float time, bool earnedMoney){
		if(customerHash.ContainsKey(customerData.customerID)){

			// Track analytics based on happy or angry leaving
			if(isLeavingHappy) {
				AnalyticsManager.Instance.CustomerLeaveHappy(satisfaction);
			}
			else {
				AnalyticsManager.Instance.CustomerLeaveAngry(customerData.type, customerData.state);
			}

			UpdateCash(satisfactionAI.CalculateBill(satisfaction, priceMultiplier, customerPos,time, earnedMoney));
			customerHash.Remove(customerData.customerID);
			CheckForGameOver();
		}
		else{
			Debug.LogError("Invalid call on " + customerData.customerID);
		}
	}

	public void UpdateCash(int billAmount){
		AudioManager.Instance.PlayClip("CoinGet");

		dayEarnedCash += billAmount;
		
		// Update revenue if positive bill
		if(billAmount > 0){
			dayCashRevenue += billAmount;
		}
	}

	//Checks to see if all the customers let and if so completes the day
	private void CheckForGameOver(){
		if(dayOver){
			if(customerHash.Count == 0){
				DataManager.Instance.GameData.DayTracker.DaysPlayed++;
				DataManager.Instance.DaysInSession++;
                if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT2"){
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 4 customers");
				}
				if(isTutorial){
					AnalyticsManager.Instance.TutorialFunnel("Finished tut day, 2 guided customers");
					DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT2";
					isTutorial = false;
					dayOver = false;
					StopCoroutine(SpawnCustomer());
					StartDay(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));
				}
				else{
					DataManager.Instance.GameData.DayTracker.AvgDifficulty = ((DataManager.Instance.GameData.DayTracker.AvgDifficulty + satisfactionAI.DifficultyLevel)/2);
					// Save data here
					int dayNetCash = dayEarnedCash + Medic.Instance.MedicCost;
					CashManager.Instance.RestaurantEndCashUpdate(dayNetCash, dayCashRevenue);

					// Unlock new event generation for StartManager
					DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
						
					// Set tutorial to done if applies
					if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1"){
						DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					}
					else if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT3"){
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

	public Table GetTable(int tableNum){
		foreach(GameObject table in tableList){
			Table tableScript = table.GetComponent<Table>();
			if(tableScript.TableNumber == tableNum){
				return tableScript;
			}
		}
		Debug.LogWarning("Can not find table " + tableNum);
		return null;
	}

	// Script is ran and cached for the first time
	public TableFlyThru GetFlyThruTable(){
		if(!isFlyThruTableCached){
			foreach(GameObject table in tableList){
				TableFlyThru tableScript = table.GetComponent<TableFlyThru>();
				if(tableScript != null){
					flyThruTable = tableScript;	// Save it to cache
				}
			}
		}
		isFlyThruTableCached = true;
		return flyThruTable;
	}

	public void DeployMedic(){
		medicTutorial.SetActive(false);
		Waiter.Instance.isMedicTut = false;
		if(SickCustomers.Count > 0){
			attempted += SickCustomers.Count;
			Medic.Instance.SetOutFromHome(SickCustomers[0].transform.position);
		}
	}

	public LineController GetLine(){
		return lineController;
	}

	public RestaurantMenuUIController GetMenuUIController(){
		return menuUIController;
	}

	public Dictionary<string, GameObject>.ValueCollection GetCurrentCustomers(){
		return customerHash.Values;
	}

	public void CustomerLineSelectHighlightOn(){
		foreach(GameObject table in tableList) {
			table.GetComponent<Table>().TurnOnHighlight();
			}
		
		if(PlayArea.Instance != null){
			PlayArea.Instance.HighLightSpots();
		}
	}

	public void CustomerLineSelectHighlightOff(){
		foreach(GameObject table in tableList) {
			table.GetComponent<Table>().TurnOffHighlight();
		}

		if(PlayArea.Instance != null){
			PlayArea.Instance.TurnOffHighLights();
		}
	}

	public void Blackout(){
		blackoutImg.SetActive(true);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i ++){
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(false);
		}
		StopCoroutine(SpawnCustomer());
		StartCoroutine(LightsOut());
	}


	private IEnumerator LightsOut(){
		yield return new WaitForSeconds(5.0f);
		blackoutImg.SetActive(false);
		List<GameObject> currCustomers = new List<GameObject>(GetCurrentCustomers());
		for(int i = 0; i < currCustomers.Count; i ++){
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(true);
		}
		StartCoroutine(SpawnCustomer());
	}

	public void SpawnSecondTut(){
		ImmutableDataCustomer test;
		test = DataLoaderCustomer.GetData("Customer10");
		GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
		GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
		cus.GetComponent<CustomerTutorial>().isAllergy = true;
		cus.GetComponent<Customer>().Init(customerNumber, eventData);
		customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
		
		customerNumber++;
		satisfactionAI.AddCustomer();
	}

	private void RunPlayAreaTut(){
		StartCoroutine("WaitASec");
		//cus.GetComponent<Customer>().JumpToTable(i);
	}

	IEnumerator WaitASec(){
		yield return (0);
		for (int i = 0; i < 4; i++){
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData(currCusSet[1]);
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

	public bool IsTableAvilable(){
		for(int i = 0; i < 4; i++){
			if(!GetTable(i).inUse){
				return true;
			}
		}
		return false;
	}


	#region PausingUI functions
	// Called from RestaurantUIManager
	public void PauseGame(){
		Time.timeScale = 0.0f;
		pauseUI.Show();
		isPaused = true;
	}

	// Called from PauseUIController
	public void ResumeGame(){
		Time.timeScale = 1.0f;
		pauseUI.Hide();
		isPaused = false;
	}

	// Called from PauseUIController
	public void QuitGame(){
		Time.timeScale = 1.0f;	// Remember to reset timescale!
		if(!dayOver){
			IncompleteQuitAnalytics();
        }

		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
	}

	// Used in OnApplicationPaused in Restaurant and quit button
	public void IncompleteQuitAnalytics() {
		AnalyticsManager.Instance.TrackGameDayInRestaurant(dayTimeLeft, TierManager.Instance.Tier, DataManager.Instance.GameData.RestaurantEvent.CurrentEvent,
				satisfactionAI.DifficultyLevel, satisfactionAI.MissingCustomers, satisfactionAI.AvgSatisfaction(),
				DayEarnedCash, Medic.Instance.MedicCost);
	}
	#endregion
}
