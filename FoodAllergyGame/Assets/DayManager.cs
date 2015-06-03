﻿using UnityEngine;
using System.Collections;

public class DayManager : MonoBehaviour {

	//Start Day begins the day coroutine and customer coroutine
	//Day Tracker once time runs out stops the customer loop
	// CustomerSpawning - spawns a customer adds it to the list
	//amount of time in seconds that the days lasts
	public float dayTime;
	//time it takes for a customer to spawn
	public float customerTimer;
	public bool dayOver =false false;
	//tracks customers via hashtable

	// RemoveCustomer removes the customer from a hashtable 
	//and then if the day is over checks to see if the hastable is empty and if it is it ends the round



	public void StartDay(){
		StartCoroutine("DayTracker");
		StartCoroutine("SpawnCustomer");
	}

	IEnumerator DayTracker(){
		while(GameManager.isPaused){
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(DayTime);
		dayOver = true;
	}

	IEnumerator SpawnCustomer(){
		while(GameManager.isPaused){
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(customerTimer);
		if(!dayOver){
			//TODO SpawnCustomer
			StartCoroutine("SpawnCustomer");
		}
	}
}
