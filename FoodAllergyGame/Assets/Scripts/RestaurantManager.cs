using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;

public class RestaurantManager : Singleton<RestaurantManager>{

	//Start Day begins the day coroutine and customer coroutine
	//Day Tracker once time runs out stops the customer loop
	// CustomerSpawning - spawns a customer adds it to the list
	//amount of time in seconds that the days lasts
	public bool isPaused;
	private float customerSpawnTimer;
	public float dayTime;			// Total amount of time in the day
	private float dayTimeLeft;		// Current amount of time left in the day

	private int customerNumber = 0;
	public float customerTimer;		//time it takes for a customer to spawn
	public bool dayOver = false;	// bool controlling customer spawning depending on the stage of the day
	public int actTables;
	public int MedicUsed;

	private int dayEarnedCash;		// The cash that is earned for the day
	public int DayEarnedCash{
		get{ return dayEarnedCash; }
	}
	
	private int dayCashRevenue;		// The total positive cashed gained for the day
	//tracks customers via hashtable
	private Dictionary<string, GameObject> customerHash;
	// our satisfaction ai 
	private SatisfactionAI satisfactionAI;
	public List<GameObject> SickCustomers;
	public GameObject[] tableList;
	public RestaurantUIManager restaurantUI;
	private ImmutableDataEvents eventData;
	public LineController Line;
	public RestaurantMenuUIController menuUIController;
	public KitchenManager kitchen;
	public bool firstSickCustomer = false;
	public GameObject medicButton;
	public GameObject medicTutorial;
	public GameObject blackoutImg;
	List<string> currCusSet;
	public bool isTutorial;
	public SpriteRenderer floorSprite;
	public TweenToggle pauseMenu;

#region Analytics
	//analytics
	private int attempted;
	public int savedCustomers;
#endregion
	// RemoveCustomer removes the customer from a hashtable 
	// and then if the day is over checks to see if the hastable is empty and if it is it ends the round

	void Start(){
		SickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();
		CustomerSeated();
		if(DataManager.Instance.isDebug){
			FoodManager.Instance.GenerateMenu(DataLoaderMenuSet.GetData("MenuSetT1").MenuSet.ToList(), 0);
			StartDay(DataLoaderEvents.GetData("Event00"));
		}
		else{
			StartDay(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));

		}
		ImmutableDataDecoItem _sprite = DataLoaderDecoItem.GetData(GetCurrentSprite(DecoTypes.Floor));
		floorSprite.sprite = Resources.Load<Sprite>(_sprite.SpriteName);
	}

	// Called at the start of the game day begins the day tracker coroutine 
	public void StartDay(ImmutableDataEvents eventData){
		Debug.Log ("StartingDay");
		this.eventData = eventData;
		string currSet = eventData.CustomerSet;
		Debug.Log (currSet);
		currCusSet = new List<string>(DataLoaderCustomerSet.GetData(currSet).CustomerSet);
		KitchenManager.Instance.Init(eventData.KitchenTimerMod);
		dayEarnedCash = 0;
		dayCashRevenue = 0;
		restaurantUI.StartDay();
		
		if(eventData.ID == "EventT1"){
			isTutorial = true;
			customerSpawnTimer = customerTimer / satisfactionAI.DifficultyLevel + 1;
		}
		else{
			dayTime = eventData.DayLengthMod;
			dayTimeLeft = dayTime;
		}
		StartCoroutine("SpawnCustomer");
	}

	// When complete flips the dayOver bool once the bool is true customers will cease spawning and the game will be looking for the end point
	void Update(){
		if(!isPaused && dayOver == false){
			dayTimeLeft -= Time.deltaTime;
			restaurantUI.UpdateClock(dayTime, dayTimeLeft);
			if(dayTimeLeft < 0)
			{
				dayOver = true;
				restaurantUI.FinishClock();
			}
		}
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer(){
		yield return new WaitForSeconds(customerSpawnTimer);
		if(isTutorial){
			ImmutableDataCustomer test;
			test = DataLoaderCustomer.GetData(currCusSet[0]);
			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			cus.GetComponent<Customer>().Init(customerNumber, eventData);
			customerHash.Add(cus.GetComponent<Customer>().customerID,cus);
			customerNumber++;
			satisfactionAI.AddCustomer();
		}
		else{
			int rand;		
			if(!dayOver && customerHash.Count < 8){
				ImmutableDataCustomer test;
//				Debug.Log (satisfactionAI.DifficultyLevel);
				customerSpawnTimer = customerTimer / satisfactionAI.DifficultyLevel+1;
				if(satisfactionAI.DifficultyLevel > 13){
					customerSpawnTimer = customerTimer / satisfactionAI.DifficultyLevel+1;
				}
				else{
					customerSpawnTimer = customerTimer / satisfactionAI.DifficultyLevel+2;
				}
					rand = UnityEngine.Random.Range(0,currCusSet.Count);
					test = DataLoaderCustomer.GetData(currCusSet[rand]);
			

				GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
				GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
				customerNumber++;
				cus.GetComponent<Customer>().Init(customerNumber, eventData);
//				rand = Random.Range(0,10);
//				if(rand > 7){
//					cus.GetComponent<Customer>().hasPowerUp = true;
//				}
				customerHash.Add(cus.GetComponent<Customer>().customerID,cus);
				satisfactionAI.AddCustomer();
				StartCoroutine("SpawnCustomer");
			}
			else{
				StartCoroutine("SpawnCustomer");
			}
		}
	}

	// Removes a customer from the dictionary
	public void CustomerLeft(string Id, int satisfaction,int priceMultiplier){
		if(customerHash.ContainsKey(Id)){
			UpdateCash(satisfactionAI.CalculateBill(satisfaction,priceMultiplier));
			customerHash.Remove(Id);
			CheckForGameOver();
		}
		else{
			Debug.LogError("Invalid call on " + Id);
		}
	}

	public void UpdateCash(int billAmount){
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
				if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT2"){
					Analytics.CustomEvent("Finished 4 Customer Tutorial", new Dictionary<string,object>{});
				}
				if(isTutorial){
					Analytics.CustomEvent("Finished First Tutorial", new Dictionary<string,object>{});
					DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT2";
					isTutorial = false;
					dayOver = false;
					StopCoroutine("SpawnCustomer");
					StartDay(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));
				}
				else{
					// Save data here
					int dayNetCash = dayEarnedCash - FoodManager.Instance.MenuCost;
					DataManager.Instance.GameData.Cash.SaveCash(dayNetCash, dayCashRevenue);

					// Unlock new event generation for StartManager
					DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
						
					// Set tutorial to done if applies
					if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1"){
						DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
					}
					else if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT3"){
						DataManager.Instance.GameData.Tutorial.IsTutorial2Done = true;
						Analytics.CustomEvent("Menu Tutorial Day Complete", new Dictionary<string, object>{});
					}
					Analytics.CustomEvent("End Of day Report", new Dictionary<string, object> {
						{"Tier", TierManager.Instance.Tier},
						{"Event", DataManager.Instance.GameData.RestaurantEvent.CurrentEvent},
						{"Missed Customers", satisfactionAI.MissingCustomers},
						{"Avg. Satisfaction", satisfactionAI.AvgSatifaction()},
						{"Cash Earned",DayEarnedCash},
						{"Cash Lost", FoodManager.Instance.MenuCost + Medic.Instance.MedicCost},
						{"Medic Saved", savedCustomers},
						{"Attempted Rescues", attempted}
					});
					// Show day complete UI
					restaurantUI.DayComplete(satisfactionAI.MissingCustomers, satisfactionAI.AvgSatifaction(), dayEarnedCash,
				 	                        FoodManager.Instance.MenuCost, dayNetCash,
				 	                        DataManager.Instance.GameData.Cash.CurrentCash,
					                         Medic.Instance.MedicCost);
				}
			}
		}
	}

	public Table GetTable(int tableNum){
		foreach(GameObject table in tableList){
			Table tableScript = table.GetComponent<Table>();
			if(tableScript.tableNumber == tableNum){
				return tableScript;
			}
		}
		Debug.LogWarning("Can not find table " + tableNum);
		return null;
	}

	public void DeployMedic(){
		RestaurantManager.Instance.medicButton.GetComponent<Animator>().SetBool("TutMedic", false);
		RestaurantManager.Instance.medicTutorial.SetActive(false);
		if(SickCustomers.Count > 0){
			attempted += SickCustomers.Count;
			Medic.Instance.MoveToLocation(SickCustomers[0].transform.position);
		}
	}

	// TEMPORARY FOR PROTOTYPE
	public void RestartGame(){
		TransitionManager.Instance.TransitionScene(SceneUtils.START);
	}

	public LineController GetLine(){
		return Line;
	}

	public RestaurantMenuUIController GetMenuUIController(){
		return menuUIController;
	}

	public KitchenManager GetKitchen(){
		return kitchen;
	}

	public Dictionary<string, GameObject>.ValueCollection getCurrentCustomers(){
		return customerHash.Values;
	}

	public void AvailableTables(){
		for (int i = 0; i < 4; i++){
			GetTable(i).TurnOnHighlight();
		}
	}

	public void CustomerSeated(){
		for (int i = 0; i < 4; i++){
			GetTable(i).TurnOffHighlight();
		}
	}

	public void Blackout(){
		blackoutImg.SetActive(true);
		List<GameObject> currCustomers = new List<GameObject>(getCurrentCustomers());
		for (int i = 0; i < currCustomers.Count; i ++){
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(false);
		}
		StopCoroutine("SpawnCustomer");
		StartCoroutine(LightsOut());
	}
	public void SpawnSecondTut(){
		ImmutableDataCustomer test;
		test = DataLoaderCustomer.GetData(currCusSet[0]);
		GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
		GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
		cus.GetComponent<CustomerTutorial>().isAllergy = true;
		cus.GetComponent<Customer>().Init(customerNumber, eventData);
		customerHash.Add(cus.GetComponent<Customer>().customerID,cus);

		customerNumber++;
		satisfactionAI.AddCustomer();
	}

	IEnumerator LightsOut(){
		yield return new WaitForSeconds(5.0f);
		blackoutImg.SetActive(false);
		List<GameObject> currCustomers = new List<GameObject>(getCurrentCustomers());
		for (int i = 0; i < currCustomers.Count; i ++){
			currCustomers[i].GetComponent<Customer>().customerUI.gameObject.SetActive(true);
		}
		StartCoroutine("SpawnCustomer");
	}

	public string GetCurrentSprite(DecoTypes deco){
		return DataManager.Instance.GameData.Decoration.currDiner[deco];
	}

	public void PauseGame(){
		pauseMenu.isUseEstimatedTime = true;
		pauseMenu.Show();
		Time.timeScale = 0.0f;
		isPaused = true;
	}

	public void UnpauseGame(){
		pauseMenu.isUseEstimatedTime = false;
		Time.timeScale = 1.0f;
		pauseMenu.Hide();
		isPaused = false;
	}

	public void QuitGame(){
		if(dayOver){
			Analytics.CustomEvent("Finished Day" , new Dictionary<string, object>{});
		}
		else{
			Analytics.CustomEvent("Quit Day" , new Dictionary<string, object>{
				{"Time Left ", dayTimeLeft},
				{"Tier", TierManager.Instance.Tier},
				{"Event", DataManager.Instance.GameData.RestaurantEvent.CurrentEvent},
				{"Difficulty Level", satisfactionAI.DifficultyLevel},
				{"Missed Customers", satisfactionAI.MissingCustomers},
				{"Avg. Satisfaction", satisfactionAI.AvgSatifaction()},
				{"Cash Earned",DayEarnedCash},
				{"Cash Lost", FoodManager.Instance.MenuCost + Medic.Instance.MedicCost},
			});
		}
		Application.LoadLevel(SceneUtils.START);
	}
}
