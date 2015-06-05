using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RestaurantManager : Singleton<RestaurantManager> {

	//Start Day begins the day coroutine and customer coroutine
	//Day Tracker once time runs out stops the customer loop
	// CustomerSpawning - spawns a customer adds it to the list
	//amount of time in seconds that the days lasts
	public float dayTime;
	//time it takes for a customer to spawn
	public float customerTimer;
	public bool dayOver = false;
	//tracks customers via hashtable
	private Dictionary<string, GameObject> customerHash;
	private SatisfactionAI satisfactionAI;

	// RemoveCustomer removes the customer from a hashtable 
	//and then if the day is over checks to see if the hastable is empty and if it is it ends the round

	void Start(){
		customerHash = new Dictionary<string, GameObject>();
		satisfactionAI = new SatisfactionAI();
	}
	// Called at the start of the game day begins the day tracker coroutine 
	public void StartDay(){
		StartCoroutine("DayTracker");
		StartCoroutine("SpawnCustomer");
	}
	// when complete flips the dayOver bool once the bool is true customers will cease spawning and the game will be looking for the end point
	IEnumerator DayTracker(){
		while(GameManager.Instance.isPaused){
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(dayTime);
		dayOver = true;
	}
	//Spawns a customer after a given amount of timer then it restarts the coroutine
	IEnumerator SpawnCustomer(){
		while(GameManager.Instance.isPaused){
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(customerTimer);
		if(!dayOver){
			//TODO SpawnCustomer
			StartCoroutine("SpawnCustomer");
		}
	}
	// removes a customer from the dictionary
	public void CustomerLeft(string Id, int satisfaction){
		if(customerHash.ContainsKey(Id)){
			satisfactionAI.CalculateCheck(satisfaction);
			customerHash.Remove(Id);
			CheckForGameOver();
		}
		else{
			Debug.LogError("Invalid call on " + Id);
		}
	}
	//checks to see if all the customers let and if so completes the day
	private void CheckForGameOver(){
		if(dayOver){
			if(customerHash.Count == 0){
				GameManager.Instance.DayComplete();
			}
		}
	}
}
