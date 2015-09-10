using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayArea : Singleton<PlayArea>, IWaiterSelection {

	public List<Transform> spotList;			// This needs to be populated, all data derived from here
	public int deltaSatisfaction = 0;			// Changes to the customers heart

	private int maxSpots;
	private List<bool> spotAvailabilityList;	// Populated at runtime, keep track of spot occupancy
	
	void Start(){
		maxSpots = spotList.Count;

		// Populate the spot availability list set them all to true
		spotAvailabilityList = new List<bool>();
		for(int i = 0; i < maxSpots; i++){
			spotAvailabilityList.Add(true);
		}
	}

	public void OnClicked(){
		int availableSpotIndex = spotAvailabilityList.IndexOf(true);
		if(Waiter.Instance.CurrentLineCustomer != null && availableSpotIndex != -1){
			Customer selectedCustomer = Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>();
			selectedCustomer.transform.localScale = Vector3.one;

			Vector3 playAreaSpot = spotList[availableSpotIndex].position;
			spotAvailabilityList[availableSpotIndex] = false;

			selectedCustomer.GoToPlayArea(playAreaSpot, availableSpotIndex, deltaSatisfaction);
		}
	}

	public bool IsQueueable(){
		return false;
	}

	public void OnWaiterArrived(){
	}

	// Called from Customer.PlayTime
	// Play time ended, reset the availability
	public void EndPlayTime(int spotIndex){
		spotAvailabilityList[spotIndex] = true;
	}
}
