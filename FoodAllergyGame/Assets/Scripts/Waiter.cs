﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
				hand1Object.transform.SetParent(hand1Parent);
				hand1Object.transform.localPosition = new Vector3(0,0,0);
				//TODO show order in hand
			}
			else{
				hand1 = WaiterHands.Meal;
				hand1Object = order;
				hand1Object.transform.SetParent(hand1Parent);
				hand1Object.transform.localPosition = new Vector3(0,0,0);
				//TODO show meal in hand
			}
		}
		else if(hand2 == WaiterHands.None){
			if(!order.GetComponent<Order>().isCooked){
				hand2 = WaiterHands.Order;
				hand2Object = order;
				hand2Object.transform.SetParent(hand2Parent);
				hand2Object.transform.localPosition = new Vector3(0,0,0);
				//TODO show order in hand
			}
			else{
				hand2 = WaiterHands.Meal;
				hand2Object = order;
				hand2Object.transform.SetParent(hand2Parent);
				hand2Object.transform.localPosition = new Vector3(0,0,0);
				//TODO show meal in hand
			}
		}
		else{
			//TODO hands are full
		}
	}

	public List <GameObject> OrderChef(){
		List <GameObject> tempOrderArr = new List<GameObject>();
		if(hand2 == WaiterHands.Order && hand1 == WaiterHands.Order){
			tempOrderArr.Add(hand2Object);
			tempOrderArr.Add(hand1Object);
			hand2 = WaiterHands.None;
			hand1 = WaiterHands.None;
			return tempOrderArr;
		}
		else if(hand2 == WaiterHands.Order){
			tempOrderArr.Add(hand2Object);
			hand2Object = null;
			hand2 = WaiterHands.None;
			return tempOrderArr;
		}
		else if(hand1 == WaiterHands.Order){
			tempOrderArr.Add(hand1Object);
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempOrderArr;
		}
		else{
			return tempOrderArr;
			//nothing here
		}
	}

	public GameObject HandMeal(int tableNum){
		if(hand1 == WaiterHands.Meal && hand1Object.GetComponent<Order>().tableNumber== tableNum){
			GameObject tempFood = hand1Object;
			tempFood.transform.SetParent(GameObject.Find("Table" + tableNum.ToString()).transform.GetChild (3));
			tempFood.transform.localPosition = new Vector3 (0,0,0);
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempFood;
		}
		else if(hand2 == WaiterHands.Meal && hand2Object.GetComponent<Order>().tableNumber == tableNum){
			GameObject tempFood = hand2Object;
			tempFood.transform.SetParent(GameObject.Find("Table" + tableNum.ToString()).transform.GetChild (3));
			tempFood.transform.localPosition = new Vector3 (0,0,0);
			hand2Object = null;
			hand2 = WaiterHands.None;
			return tempFood;
		}
		//else{
			return hand1Object;
			// do nothing
		//}
	}

	public bool CheckHands(){
		if(hand2 == WaiterHands.None || hand1 == WaiterHands.None){
			return true;
		}
		else{
			return false;
		}
	}

	public void WriteDownOrder(GameObject order){
		SetHand(order);
		GameObject.Find ("Table"+ order.GetComponent<Order>().tableNumber.ToString ()).GetComponent<Table>().OrderObtained();
	}
}
