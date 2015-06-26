using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RestaurantManager : Singleton<RestaurantManager>{

	//Start Day begins the day coroutine and customer coroutine
	//Day Tracker once time runs out stops the customer loop
	// CustomerSpawning - spawns a customer adds it to the list
	//amount of time in seconds that the days lasts
	public bool isPaused;

	public float dayTime;
	private int customerNumber = 0;
	public float customerTimer;		//time it takes for a customer to spawn
	public bool dayOver = false;	// bool controlling customer spawning depending on the stage of the day

	private float dayCash;			// The cash that is earn for the day
	public float DayCash{
		get{ return dayCash; }
	}

	//tracks customers via hashtable
	private Dictionary<string, GameObject> customerHash;
	// our satisfaction ai 
	private SatisfactionAI satisfactionAI;

	public List<GameObject> SickCustomers;
	public GameObject[] tableList;
	public RestaurantUIManager restaurantUI;
	public string currEvent;
	private ImmutableDataEvents eventParam;

	// RemoveCustomer removes the customer from a hashtable 
	// and then if the day is over checks to see if the hastable is empty and if it is it ends the round

	void Start(){
		currEvent = DataManager.Instance.GetEvent();
		SickCustomers = new List<GameObject>();
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();
		Debug.Log (currEvent);
		ImmutableDataEvents test = DataLoaderEvents.GetData(currEvent);
		StartDay(test);
//		FoodManager.Instance.GenerateMenu("Food00");
//		FoodManager.Instance.GenerateMenu("Food01");
//		FoodManager.Instance.GenerateMenu("Food02");
//		FoodManager.Instance.GenerateMenu("Food03");
//		FoodManager.Instance.GenerateMenu("Food04");
	}

	// Called at the start of the game day begins the day tracker coroutine 
	public void StartDay(ImmutableDataEvents mode){
		eventParam = mode;
		switch (mode.DayMod){
		case "0":
			//dayTime = dayTime;
			break;
		case "1":
			dayTime = dayTime * 2;
			break;
		case "2":
			dayTime = dayTime /2;
			break;
		}

		dayCash = 0;

		StartCoroutine("DayTracker");
		StartCoroutine("SpawnCustomer");
		KitchenManager.Instance.Init(mode.KitchenMod);
	}

	// When complete flips the dayOver bool once the bool is true customers will cease spawning and the game will be looking for the end point
	IEnumerator DayTracker(){
		while(isPaused){
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(dayTime);
		dayOver = true;
	}

	// Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer(){
		while(isPaused){
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(customerTimer);
		if(!dayOver && customerHash.Count < 8){
			ImmutableDataCustomer test;
			if(satisfactionAI.GetSatisfaction() > 13){
				int rand = Random.Range(0,5);
				switch(rand){
				case 0:
				 	test = DataLoaderCustomer.GetData("Customer01");
					break;
				case 1:
					test = DataLoaderCustomer.GetData("Customer02");
					break;
				case 2: 
					test = DataLoaderCustomer.GetData("Customer03");
					break;
				case 3: 
					test = DataLoaderCustomer.GetData("Customer04");
					break;
				case 4: 
					test = DataLoaderCustomer.GetData("Customer05");
					break;

				default:
					test = DataLoaderCustomer.GetData("Customer02");
					break;
				}
			}
			else{
				 test = DataLoaderCustomer.GetData("Customer00");
			}

			GameObject customerPrefab = Resources.Load(test.Script) as GameObject;
			GameObject cus = GameObjectUtils.AddChild(null, customerPrefab);
			customerNumber++;
			cus.GetComponent<Customer>().Init(customerNumber, eventParam);
			customerHash.Add(cus.GetComponent<Customer>().customerID,cus);
			//TODO SpawnCustomer
			StartCoroutine("SpawnCustomer");
		}
		else{
			StartCoroutine("SpawnCustomer");
		}
	}

	// Removes a customer from the dictionary
	public void CustomerLeft(string Id, int satisfaction){
		if(customerHash.ContainsKey(Id)){
			Debug.Log (satisfactionAI.CalculateCheck(satisfaction));
			UpdateCash(satisfactionAI.CalculateCheck(satisfaction));
			customerHash.Remove(Id);
			CheckForGameOver();
		}
		else{
			Debug.LogError("Invalid call on " + Id);
		}
	}

	public void UpdateCash(float money){
		dayCash += money;
		restaurantUI.UpdateCash(dayCash);
	}

	//Checks to see if all the customers let and if so completes the day
	private void CheckForGameOver(){
		if(dayOver){
			if(customerHash.Count == 0){
				//TODO GameManager.Instance.DayComplete();
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
		if(SickCustomers.Count > 0){
			Medic.Instance.MoveToLocation(SickCustomers[0].transform.position);
		}
	}

	// TEMPORARY FOR PROTOTYPE
	public void RestartGame(){
		TransitionManager.Instance.TransitionScene(SceneUtils.START);
	}
}
