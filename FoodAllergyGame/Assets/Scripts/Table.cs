using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour {

	//Handles the interaction between the customer and the waiter telling the waiter what action to perform based on the customers state
	//Table Number hard coded number to distinguish between tables
	public int tableNumber;

	public void TalkToConsumer(){
		if(transform.childCount > 0){
			transform.GetChild(0).gameObject.GetComponent<Customer>().CheckState();
		}
	}

	// tell waiter to put food down
	public void FoodDelivered(){

	}
	// pay the waiter
	public void OrderObtained(){
	}
}
