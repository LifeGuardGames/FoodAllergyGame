using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour{
	// ID an id given on creation
	// Select food keyword based off allergen and random rolls
	// Allergen random between wheat, dairy and peanut
	// State the current state of the customer
	// NotifyLeave gives the user money based of satifation and then passes the id to the dayManager

	// The customer's id used for identification in the 
	public string customerID;

	// The current state of the customer
	public CustomerStates state;

	// The allergy of the customer
	public List <Allergies> allergy;

	// Time spent looking at the menu
	private float menuTimer = 4.0f;

	// The attention timer
	private float attentionSpan;

	// The satisfaction the customer has, everytime the attention span ticks down to 0 the customer will lose satisfaction
	public int satisfaction;

	// Basic intitialzation 
	public void Init(){
		state = CustomerStates.InLine;
		StartCoroutine(SatisfactionTimer());
		allergy = new List<Allergies>();
		satisfaction = 3;
	}

	// When completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		yield return new WaitForSeconds(attentionSpan);
		satisfaction--;
		StartCoroutine(SatisfactionTimer());
	}

	// JumpToTable jumps to the table given a table number
	public void JumpToTable(int tableNum){
		//TODO parent customer to table and move customer to table
		state = CustomerStates.ReadingMenu;
		StartCoroutine("ReadMenu");
		StopCoroutine(SatisfactionTimer());
		attentionSpan = 8.0f;
		StartCoroutine(SatisfactionTimer());
	}

	// Time spent reading menu before ordering
	IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);
		//TODO select food option
		StopCoroutine(SatisfactionTimer());
		attentionSpan = 8.0f;
		StartCoroutine(SatisfactionTimer());
		state = CustomerStates.WaitForOrder;
		//TODO show customer waiting for order
	}

	// Gives the order to the waiter
	public void GetOrder(){
		//TODO return the supplied order
		//TODO display table number on table
		transform.GetComponentInParent<Table>().OrderObtained();
		attentionSpan = 16.0f;
		state = CustomerStates.WaitForFood;
		StartCoroutine(SatisfactionTimer());
	}

	// Tells the waiter the food has been delivered and begins eating
	public void Eating(){
		transform.GetComponentInParent<Table>().FoodDelivered();
		StopCoroutine(SatisfactionTimer());
		state = CustomerStates.Eating;
		StartCoroutine("EatingTime");
	}

	// Eating coroutine
	IEnumerator EatingTimer(){
		yield return new WaitForSeconds(6.0f);
		attentionSpan = 6.0f;
		state = CustomerStates.WaitForCheck;
		StartCoroutine(SatisfactionTimer());
	}

	// Tells the resturantManager that the customer is leaving and can be removed from the dictionary
	public void NotifyLeave(){
		RestaurantManager.Instance.CustomerLeft(customerID, satisfaction);
	}

	// Checks the current state and runs the appropriate function called by table when waiter approaches
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
