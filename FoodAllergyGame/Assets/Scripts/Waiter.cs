using UnityEngine;
using System.Collections;

public class Waiter : MonoBehaviour{

	//MoveToLocation moves the waiter to a specified location
	//SetHand populates first free had with enum
	//ResetHand restores hand to empty state
	//UnHand hands off the item to the kitchen or customer
	public WaiterHands hand1;
	public WaiterHands hand2;
	public Order hand1Object;
	public Order hand2Object;
	public bool moving;
	private Vector3 target;
	public float speed;

	public void MoveToLocation(Vector3 location){
		moving = true;
		target = location;
	}

	void Update(){
		if(moving && Vector3.Distance(transform.position, target) > 5){
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

	public void SetHand(Order order){
		if(hand1 == WaiterHands.None){
			if(hand1Object.name == "Order"){
				hand1 = WaiterHands.Order;
				//TODO show order in hand
			}
			else{
				hand1 = WaiterHands.Meal;
				//TODO show meal in hand
			}
		}
		else if(hand2 == WaiterHands.None){
			if(hand2Object.name == "Order"){
				hand2 = WaiterHands.Order;
				//TODO show order in hand
			}
			else{
				hand2 = WaiterHands.Meal;
				//TODO show meal in hand
			}
		}
		else{
			//TODO hands are full
		}
	}

	public Order OrderChef(){
		if(hand2 == WaiterHands.Order){
			return hand2Object;
		}
		else if(hand1 == WaiterHands.Order){
			return hand1Object;
		}
		else{
			return null;
			//nothing here
		}
	}

	public Order HandMeal(int tableNum){
		if(hand1 == WaiterHands.Meal){
			//if(handROBJ.GetComponent<Order>().TableNum == tableNum){
			return hand1Object;
			//}
		}
		else if(hand2 == WaiterHands.Meal){
			//	if (handLOBJ.GetComponent<Order>().TableNum == tableNum){
			return hand2Object;
			//}
		}
		else{
			return null;
			// do nothing
		}
	}
}
