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
	public bool isFlythrough;
	public Transform waiterSpot;
	public Transform foodSpot;
	public bool inUse = false;
	public string currentCustomerID;
	public bool isBroken;
	public GameObject _canvas;
	public bool isGossiped;
	public GameObject node;
	public GameObject tableHighlight;

	//facilitates talk between customer and waiter
	public void TalkToConsumer(){

		if(inUse){
			// CheckState will handle waiter finish
			transform.GetComponentInChildren<Customer>().CheckState();
		}
		else{	// Nothing to do here
			Waiter.Instance.Finished();
		}
	}

	// tell waiter to put food down
	public GameObject FoodDelivered(){
		_canvas.SetActive(false);
		return Waiter.Instance.HandMeal(tableNumber);
	}

	//Passes order drom customer to waiter
	public void OrderObtained(GameObject order){
		Waiter.Instance.WriteDownOrder(order);
		_canvas.SetActive(true);
	}

	//makes sure there is no left over food should a customer leave ealy
	public void CustomerLeaving(){
		inUse = false;
		Waiter.Instance.RemoveMeal(tableNumber);
		RestaurantManager.Instance.GetKitchen().CancelOrder(tableNumber);
	}

	//in the unfortunate circumstance a customer gets eaten we need to take care of the mess
	public void CustomerEaten(){
		if(foodSpot.childCount > 0){
			Destroy(foodSpot.GetChild(0));
		}
		RestaurantManager.Instance.GetMenuUIController().CancelOrder(tableNumber);
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
			Waiter.Instance.currentTable = tableNumber;
			TalkToConsumer();

		}
		else{
			Waiter.Instance.Finished();
		}
	}

	public bool IsQueueable(){
		if(inUse){
			return true;
		}
		else{
			return false;
		}
	}

	public void TurnOnHighlight(){
		if(!inUse){
			tableHighlight.SetActive(true);
		}
	}

	public void TurnOffHighlight(){
		tableHighlight.SetActive(false);
	}

	public void OnClicked(){
//		if(!TouchManager.IsHoveringOverGUI()){
			// Check if customers need to jump to the table
		if(!isFlythrough){
			if(Waiter.Instance.currentLineCustomer != null && !inUse && !isBroken){
				Waiter.Instance.currentLineCustomer.transform.localScale = Vector3.one;
				Waiter.Instance.currentLineCustomer.GetComponent<Customer>().JumpToTable(tableNumber);
				inUse = true;
			}

			else if(isGossiped){
			waiterSpot.GetChild(0).GetComponent<CustomerGossiper>().GoAway();
			isGossiped = false;
		}
			// Move the waiter to the table to do what it does
			else{
				Waiter.Instance.FindRoute(node,this);
//				Waiter.Instance.MoveToLocation(waiterSpot.position, this);
			}
		}
		else{
			Waiter.Instance.FindRoute(node,this);
//			Waiter.Instance.MoveToLocation(waiterSpot.position, this);
		}
//		}
	}
	#endregion
}
