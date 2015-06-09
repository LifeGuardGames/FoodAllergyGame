﻿using UnityEngine;
using System.Collections;

public class Waiter : Singleton<Waiter>{

	//MoveToLocation moves the waiter to a specified location
	//SetHand populates first free had with enum
	//ResetHand restores hand to empty state
	//UnHand hands off the item to the kitchen or customer
	public WaiterHands hand1;
	public WaiterHands hand2;

	public Transform hand1Parent;
	public Transform hand2Parent;

	public GameObject hand1Object;
	public GameObject hand2Object;

	public bool moving;
	private Vector3 target;
	public float speed;
	public GameObject currentlyServing;

	public void MoveToLocation(Vector3 location){
		moving = true;
		target = location;
	}

	void Update(){
		if(moving && Vector3.Distance(transform.position, target) > 0){
			transform.position = Vector3.MoveTowards(transform.position, target, speed);
		}
	}

	void Start(){
		ResetHands();
	}

	public void ResetHands(){
		hand1 = WaiterHands.None;
		hand2 = WaiterHands.None;
	}

	public void SetHand(GameObject order){
		if(hand1 == WaiterHands.None){
			if(!order.GetComponent<Order>().isCooked){
				hand1 = WaiterHands.Order;
				hand1Object = order;
				//TODO show order in hand
			}
			else{
				hand1 = WaiterHands.Meal;
				hand1Object = order;
				//TODO show meal in hand
			}
		}
		else if(hand2 == WaiterHands.None){
			if(!order.GetComponent<Order>().isCooked){
				hand2 = WaiterHands.Order;
				hand2Object = order;
				//TODO show order in hand
			}
			else{
				hand2 = WaiterHands.Meal;
				hand2Object = order;
				//TODO show meal in hand
			}
		}
		else{
			//TODO hands are full
		}
	}

	public GameObject OrderChef(){
		if(hand2 == WaiterHands.Order){
			GameObject tempOrder = hand2Object;
			hand1Object = null;
			hand2 = WaiterHands.None;
			return tempOrder;
		}
		else if(hand1 == WaiterHands.Order){
			GameObject tempOrder = hand1Object;
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempOrder;
		}
		else{
			return null;
			//nothing here
		}
	}

	public GameObject HandMeal(int tableNum){
		if(hand1 == WaiterHands.Meal){
			//if(handROBJ.GetComponent<Order>().TableNum == tableNum){
			GameObject tempFood = hand1Object;
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempFood;
			//}
		}
		else if(hand2 == WaiterHands.Meal){
			//	if (handLOBJ.GetComponent<Order>().TableNum == tableNum){
			GameObject tempFood = hand2Object;
			hand1Object = null;
			hand2 = WaiterHands.None;
			return tempFood;
			//}
		}
		else{
			return null;
			// do nothing
		}
	}
}
