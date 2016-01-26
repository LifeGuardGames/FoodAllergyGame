using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Analytics;
using System;

public abstract class RestaurantManager : Singleton<RestaurantManager>{
	public static float customerLeaveModifierTime = 30f;	// When player error, notify leave will use this value for balancing

	public bool isPaused;
	protected float customerSpawnTimer;
	public float dayTime;						// Total amount of time in the day
	protected float dayTimeLeft;				// Current amount of time left in the day
	public int lineCount = 0;
	protected int customerNumber = 0;
	public bool dayOver = false;				// bool controlling customer spawning depending on the stage of the day
	public int actTables;
	public int medicUsed;
	protected int dayEarnedCash;				// The cash that is earned for the day
	public int DayEarnedCash{
		get{ return dayEarnedCash; }
	}
	
	protected int dayCashRevenue;				// The total positive cashed gained for the day
	//tracks customers via hashtable
	public  Dictionary<string, GameObject> customerHash;
	// our satisfaction ai 
	protected SatisfactionAI satisfactionAI;
	public List<GameObject> sickCustomers;

	protected List<GameObject> tableList = new List<GameObject>();
	public List<GameObject> TableList{
		get{ return tableList; }
	}

	protected TableFlyThru flyThruTable = null;		// Cached table position
	private bool isFlyThruTableCached = false;

	public RestaurantUIManager restaurantUI;
	protected ImmutableDataEvents eventData;
	public LineController lineController;
	public RestaurantMenuUIController menuUIController;
	public DoorController doorController;

	public bool firstSickCustomer = false;
	public GameObject medicButton;
	public GameObject medicTutorial;
	public GameObject blackoutImg;
	protected List<string> currCusSet;
	public bool isTutorial;
	public bool isVIPTut;
	public PauseUIController pauseUI;
	public float baseCustomerTimer;
	public float customerTimerDiffMod;

	#region Analytics
	public int inspectionButtonClicked;
	protected int attempted;
	public int savedCustomers;

	protected int playAreaUses;
	public int PlayAreaUses{
		set{ playAreaUses = value; }
		get{ return playAreaUses; }
	}

	protected int vipUses;
	public int VIPUses{
		set{ vipUses = value; }
		get{ return vipUses; }
	}

	protected int microwaveUses;
	public int MicrowaveUses{
		set{ microwaveUses = value; }
		get{ return microwaveUses; }
	}
	#endregion

	private object Manager;

	public abstract void Init();
	
	// Called at the start of the game day begins the day tracker coroutine 
	public virtual void StartDay(){
    }

	// When complete flips the dayOver bool once the bool is true customers will cease spawning and the game will be looking for the end point




	/// <summary>
	/// Logic to calculate customer leaving because of a change in satisfaction
	/// </summary>
	public virtual void CustomerLeftSatisfaction(Customer customerData, bool isModifiesDifficulty, int VIPMultiplier = 1) {
	}

	/// <summary>
	/// Logic to calculate customer leaving because of allergies (Go to hospital AND Saved)
	/// </summary>
	public virtual void CustomerLeftFlatCharge(Customer customerData, int deltaCoins, bool isModifiesDifficulty) {
	}

	// TODO Both positive earning and medic bills should go through here
	public virtual void UpdateCash(int billAmount, Vector3 customerPosition) {
		restaurantUI.UpdateCashUI(customerPosition, billAmount);

		dayEarnedCash += billAmount;
		
		// Update revenue if positive bill
		if(billAmount > 0){
			dayCashRevenue += billAmount;
		}
	}

	//Checks to see if all the customers let and if so completes the day
	protected virtual void CheckForGameOver(){
		
	}

	public virtual Table GetTable(int tableNum){
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
	public virtual TableFlyThru GetFlyThruTable(){
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

	public virtual void DeployMedic(){
		medicTutorial.SetActive(false);
		Waiter.Instance.isMedicTut = false;
		if(sickCustomers.Count > 0){
			attempted += sickCustomers.Count;
			Medic.Instance.SetOutFromHome(sickCustomers[0].transform.position);
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

	public virtual void Blackout(){
		
	}

	public void SpawnSecondTut(){
		ImmutableDataCustomer test;
		test = DataLoaderCustomer.GetData("Customer10");
		GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
		GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
		cus.GetComponent<CustomerTutorial>().isAllergy = true;
		cus.GetComponent<Customer>().behavFlow = test.BehavFlow;
		cus.GetComponent<Customer>().Init(customerNumber, eventData);
		customerHash.Add(cus.GetComponent<Customer>().customerID, cus);
		
		customerNumber++;
		satisfactionAI.AddCustomer();
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
