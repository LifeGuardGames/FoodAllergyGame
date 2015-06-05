using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {


	//ID an id given on creation
	//select food keyword based off allergen and random rolls
	// Allergen random between wheat, dariy and peanut
	//State the current state of the customer
	//NotifyLeave gives the user money based of satifation and then passes the id to the dayManager

	// The customer's id used for identification in the 
	public string customerID;
	// The current state of the customer
	public CustomerStates state;
	// the allergy of the customer
	public string allergy;
	// Time spent looking at the menu
	private float menuTimer;
	// the attention timer
	private float attentionSpan;
	//The satisfaction the customer has, everytime the attention span ticks down to 0 the customer will lose satisfaction
	public int Satisfaction;

	//Basic intitialzation 
	public void Init(){
		state = CustomerStates.InLine;
		StartCoroutine(SatisfactionTimer());
		Satisfaction = 3;
	}
	// when completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		yield return new WaitForSeconds (attentionSpan);
		Satisfaction--;
		StartCoroutine(SatisfactionTimer());
	}

	//JumpToTable jumps to the table given a table number
	public void JumpToTable(int tableNum){
		//TODO parent customer to table and move customer to table
		state = CustomerStates.ReadingMenu;
		StartCoroutine ("ReadMenu");
		StopCoroutine(SatisfactionTimer());
		attentionSpan = 8.0f;
		StartCoroutine(SatisfactionTimer());
	}
	// time spent reading menu before ordering
	IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);
		//TODO select food option
		state = CustomerStates.WaitForOrder;
	}
	// gives the order to the waiter
	public void GetOrder(){
		//TODO return the supplied order
		//TODO display table number on table
		transform.GetComponentInParent<Table>().OrderObtained();
		attentionSpan = 16.0f;
		state = CustomerStates.WaitForFood;
		StartCoroutine(SatisfactionTimer());
	}
	// tells the waiter the food has been delivered and begins eating
	public void Eating(){
		transform.GetComponentInParent<Table>().FoodDelivered();
		StopCoroutine(SatisfactionTimer());
		state = CustomerStates.Eating;
		StartCoroutine("EatingTime");
	}
	// eating coroutine
	IEnumerator EatingTimer(){
		yield return new WaitForSeconds(6.0f);
		attentionSpan = 6.0f;
		state = CustomerStates.WaitForCheck;
		StartCoroutine(SatisfactionTimer());
	}
	// tells the resturantManager that the customer is leaving and can be removed from the dictionary
	public void NotifyLeave(){
		RestaurantManager.Instance.CustomerLeft(customerID,Satisfaction);
	}
	//checks the current state and runs the appropriate function called by table when waiter approaches
	public void CheckState(){
		switch(state){
		case CustomerStates.WaitForOrder:
			GetOrder();
			break;
		case CustomerStates.WaitForFood:
			Eating();
			break;
		case CustomerStates.WaitForCheck:
			NotifyLeave();
			break;
		default:
			break;
		}
	}

}
