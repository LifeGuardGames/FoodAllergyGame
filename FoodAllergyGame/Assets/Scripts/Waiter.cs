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
	public float movingTime = 0.3f;
	public GameObject currentLineCustomer;
	public bool canMove = true;
	public GameObject currentNode;
	private List<GameObject> pathList;
	private int index = 0;

	public WaiterAnimController waiterAnimController;

	private IWaiterSelection currentCaller;

	void Start(){
		ResetHands();
		pathList = new List<GameObject>();
	}
	public void FindRoute(GameObject targetNode, MonoBehaviour caller){
		currentCaller = (IWaiterSelection)caller;
		pathList.Clear();
		if(currentCaller == null){
			Debug.LogError("No IWaiterSelection script exists in the caller");
		}
		if(currentNode == targetNode){
			MoveDoneCallback();
		}
		else{
			pathList = Pathfinding.Instance.findPath(currentNode, targetNode);
			currentNode = targetNode;
			MoveToLocation(pathList, index);
		}
	}
	public void MoveToLocation(List<GameObject> path, int index){
			canMove = false;
//			currentCaller = (IWaiterSelection)caller;
//			if(currentCaller == null){
//				Debug.LogError("No IWaiterSelection script exists in the caller");
//				}
		
			//If the waiter is already at its location, just call what it needs to call
//			if(transform.position == location){
//				MoveDoneCallback();
//			}
			// Otherwise, move to the location and wait for callback
//			else{
			moving = true;
			waiterAnimController.SetMoving(true);
//			Debug.Log (path[index].name);
			LeanTween.cancel(gameObject);
			LeanTween.move(gameObject, path[index].transform.position, movingTime).setOnComplete(MoveDoneCallback);
//			}
	}
	public void MoveDoneCallback(){
		if(pathList.Count == 0){
			if(currentCaller == null){
				Debug.LogError("No IWaiterSelection script currently exists");
			}
			currentCaller.OnWaiterArrived();
		}
		else if(currentNode == pathList[index]){
			index = 0;
			// Note: Set animations to false before OnWaiterArrived
			moving = false;
			waiterAnimController.SetMoving(false);

			if(currentCaller == null){
				Debug.LogError("No IWaiterSelection script currently exists");
			}
			currentCaller.OnWaiterArrived();
		}
		else{
			//canMove = true;
			index++;
			MoveToLocation(pathList, index);
		}
	}


	public void ResetHands(){
		hand1 = WaiterHands.None;
		hand2 = WaiterHands.None;
	}

	public void SetHand(GameObject order){
		if(hand1 == WaiterHands.None){
			if(!order.GetComponent<Order>().IsCooked){
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
			if(!order.GetComponent<Order>().IsCooked){
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

	public GameObject QuickCook(){
		GameObject tempOrder;
		tempOrder = null;
		if(hand1 == WaiterHands.Order){
			tempOrder = hand1Object;
			return tempOrder;
		}
		else if (hand2 == WaiterHands.Order){
			tempOrder = hand2Object;
			return tempOrder;
		}
		else{
			return tempOrder;
		}

	}

	public GameObject HandMeal(int tableNum){
		if(hand1 == WaiterHands.Meal && hand1Object.GetComponent<Order>().tableNumber== tableNum){
			GameObject tempFood = hand1Object;
			tempFood.transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).foodSpot);
			tempFood.transform.localPosition = new Vector3 (0,0,0);
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempFood;
		}
		else if(hand2 == WaiterHands.Meal && hand2Object.GetComponent<Order>().tableNumber == tableNum){
			GameObject tempFood = hand2Object;
			tempFood.transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).foodSpot);
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

	public void RemoveMeal(int table){
		if(hand1 != WaiterHands.None){
			if(hand1Object.GetComponent<Order>().tableNumber == table){
				Destroy(hand1Object.gameObject);
				hand1 = WaiterHands.None;
			}
		}
		if(hand2 != WaiterHands.None){
			if(hand2Object.GetComponent<Order>().tableNumber == table){
				Destroy(hand2Object.gameObject);
				hand2 = WaiterHands.None;
			}
		}
	}

	public bool HaveMeal(int table){
		if(hand1 != WaiterHands.None){
			if(hand1Object.GetComponent<Order>().tableNumber == table && hand1Object.GetComponent<Order>().IsCooked ){
				//Debug.Log ("Hand1");
				return true;
			}
		}
		if(hand2 != WaiterHands.None){
			if(hand2Object.GetComponent<Order>().tableNumber == table && hand2Object.GetComponent<Order>().IsCooked ){
				return true;
				//Debug.Log("Hand2");
			}
		}
			return false;
	}

	public void GivePowerUp(){
		
	}

	public void WriteDownOrder(GameObject order){
		AudioManager.Instance.PlayClip("writeDownOrder");
		SetHand(order);
	}

	public void Finished(){
		canMove = true;
		if(TouchManager.Instance.inputQueue.Count > 0){
		//	if(TouchManager.Instance.inputQueue.Peek ().GetComponent<Table>().seat.GetComponentInChildren<Customer>().state != CustomerStates.WaitForOrder && Waiter.Instance.CheckHands()){
			GameObject dequeuedObject = TouchManager.Instance.inputQueue.Dequeue();
			if(dequeuedObject != null){
				dequeuedObject.GetComponent<IWaiterSelection>().OnClicked();
			}
			//}
		}
	}
}
