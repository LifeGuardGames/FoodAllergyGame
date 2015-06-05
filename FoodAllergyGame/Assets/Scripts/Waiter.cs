using UnityEngine;
using System.Collections;

public class Waiter : MonoBehaviour {

	//MoveToLocation moves the waiter to a specified location
	//SetHand populates first free had with enum
	//ResetHand restores hand to empty state
	//UnHand hands off the item to the kitchen or customer
	public WaiterHands handL;
	public WaiterHands handR;
	public GameObject handLOBJ;
	public GameObject handROBJ;
	public bool moving;
	private Vector3 target;
	public float speed;

	public void MoveToLocation(Vector3 location){
		moving = true;
		target = location;
	}

	void Update(){
		if(moving && Vector3.Distance(transform.position , target) > 5){
			transform.position = Vector3.MoveTowards(transform.position, target,speed);
		}
	}

	// Use this for initialization
	void Start () {
		handL = WaiterHands.None;
		handR = WaiterHands.None;
	}

	public void setHand(object thing){
		if(handR == WaiterHands.None){
			if(handROBJ.name == "order"){
				handR = WaiterHands.Order;
				//TODO show order in hand
			}
			else{
				handR = WaiterHands.Meal;
				//TODO show meal in hand
			}
		}
		else if (handL ==  WaiterHands.None){
			if(handLOBJ.name == "order"){
				handL = WaiterHands.Order;
				//TODO show order in hand
			}
			else{
				handL = WaiterHands.Meal;
				//TODO show meal in hand
			}
		}
		else{
		//TODO hands are full
		}
	}

	public void ResetHands(){
		handL = WaiterHands.None;
		handR = WaiterHands.None;
	}

	public GameObject OrderChef(){
		if(handL == WaiterHands.Order){
			return handLOBJ;
		}
		else if (handR == WaiterHands.Order){
			return handROBJ;
		}
		else{
			return null;
		//nothing here
		}
	}

	public GameObject HandMeal(int tableNum){
		if(handR == WaiterHands.Meal){
			//if(handROBJ.GetComponent<Order>().TableNum == tableNum){
				return handROBJ;
			//}
		}
		else if(handL == WaiterHands.Meal){
		//	if (handLOBJ.GetComponent<Order>().TableNum == tableNum){
				return handLOBJ;
			//}
		}

		else{
			return null;
			// do nothing
			}

	}

}
