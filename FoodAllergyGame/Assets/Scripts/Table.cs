using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour {

	//Handles the interaction between the customer and the waiter telling the waiter what action to perform based on the customers state
	//Table Number hard coded number to distinguish between tables
	public int tableNumber;

	public Transform seat;
	public Transform waiterSpot;
	public Transform foodSpot;
	public bool inUse = false;

	public void TalkToConsumer(){
		if(inUse){
			transform.GetComponentInChildren<Customer>().CheckState();
		}
	}

	// tell waiter to put food down
	public void FoodDelivered(){
		Waiter.Instance.HandMeal(tableNumber);
	}
	// pay the waiter
	public void OrderObtained(){
	}
}
