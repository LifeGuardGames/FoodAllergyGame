﻿using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour, IWaiterSelection{

	//Handles the interaction between the customer and the waiter telling the waiter what action to perform based on the customers state
	//Table Number hard coded number to distinguish between tables
	public int tableNumber;

	public Transform seat;
	public Transform Seat{
		get{ return seat; }
	}

	public Transform waiterSpot;
	public Transform foodSpot;
	public bool inUse = false;
	public string currentCustomerID;
	public bool isBroken;

	//facilitates talk between customer and waiter
	public void TalkToConsumer(){
		if(inUse){
			transform.GetComponentInChildren<Customer>().CheckState();
		}
		if(seat.childCount > 0){
			if(seat.GetComponentInChildren<Customer>().state != CustomerStates.WaitForOrder && Waiter.Instance.CheckHands()){
				Waiter.Instance.Finished();
			}
			else if(seat.GetComponentInChildren<Customer>().state == CustomerStates.WaitForCheck){
				Waiter.Instance.Finished();
			}
		}
		else{
			Waiter.Instance.Finished();
		}
	}

	// tell waiter to put food down
	public GameObject FoodDelivered(){
		return Waiter.Instance.HandMeal(tableNumber);
	}

	//Passes order drom customer to waiter
	public void OrderObtained(GameObject order){
		Waiter.Instance.WriteDownOrder(order);
	}

	//makes sure there is no left over food should a customer leave ealy
	public void CustomerLeaving(){
		inUse = false;
		Waiter.Instance.RemoveMeal(tableNumber);
		GameObject.Find("Kitchen").GetComponent<KitchenManager>().CancelOrder(tableNumber);
	}

	//in the unfortunate circumstance a customer gets eaten we need to take care of the mess
	public void CustomerEaten(){
		if(foodSpot.childCount > 0){
			Destroy (foodSpot.GetChild (0));
		}
		GameObject.Find("MenuUIManager").GetComponent<MenuUIManager>().CancelOrder(tableNumber);
		GetComponentInChildren<Customer>().state = CustomerStates.Invalid;
		RestaurantManager.Instance.CustomerLeft(currentCustomerID, 0);
		CustomerLeaving();
	}
	//for use by sir table smasher when he does his thing
	public void TableSmashed(){
		isBroken = true;
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(!isBroken &&seat.childCount > 0){
			TalkToConsumer();
		}
		else{
			Waiter.Instance.Finished();
		}
	}
	public void OnClicked(){
		// Check if customers need to jump to the table
		if(Waiter.Instance.currentLineCustomer != null && !inUse && !isBroken){
			Waiter.Instance.currentLineCustomer.transform.localScale = Vector3.one;
			Waiter.Instance.currentLineCustomer.GetComponent<Customer>().JumpToTable(tableNumber);
			inUse = true;
		}
		// Move the waiter to the table to do what it does
		else{
			Waiter.Instance.MoveToLocation(waiterSpot.position, this);
		}
	}
	#endregion
}
