﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Microwave :Singleton<Microwave>, IWaiterSelection{

	public float cookTimer;
	public Transform waiterSpot;
	public bool isCooking;
	public GameObject currentlyCooking;

	public void CookOrder(GameObject order){
		if( order != null){
			currentlyCooking = order;
			order.transform.SetParent(this.gameObject.transform);
			order.GetComponent<Order>().StartCooking(cookTimer);
			isCooking = true;
			//AnimSetCooking(1);
			//AudioManager.Instance.PlayClip("giveOrder");
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(isCooking){
			Waiter.Instance.SetHand(currentlyCooking);
		}
		else{
			CookOrder(Waiter.Instance.QuickCook());
		}
		Waiter.Instance.Finished();
	}
	
	public void OnClicked(){
		//		if(!TouchManager.IsHoveringOverGUI()){
		Waiter.Instance.MoveToLocation(waiterSpot.position, this);
		//		}
	}
	#endregion

}
