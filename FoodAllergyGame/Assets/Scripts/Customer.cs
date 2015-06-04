using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {


	//ID an id given on creation
	//select food keyword based off allergen and random rolls
	// Allergen random between wheat, dariy and peanut
	//State the current state of the customer

	//NotifyLeave gives the user money based of satifation and then passes the id to the dayManager

	public string customerID;
	public CustomerStates state;
	public string allergy;
	private float menuTimer;
	private float attentionSpan;
	public int Satisfaction;

	public void Init(){
		state = CustomerStates.InLine;
		StartCoroutine(SatisfactionTimer());
		Satisfaction = 3;
	}

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

	IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);
		//TODO select food option
		state = CustomerStates.WaitForOrder;
	}

	public void GetOrder(){
		//TODO return the supplied order
		//TODO display table number on table
		transform.GetComponentInParent<Table>().OrderObtained();
		attentionSpan = 16.0f;
		state = CustomerStates.WaitForFood;
		StartCoroutine(SatisfactionTimer());
	}

	public void Eating(){
		transform.GetComponentInParent<Table>().FoodDelivered();
		StopCoroutine(SatisfactionTimer());
		state = CustomerStates.Eating;
		StartCoroutine("EatingTime");
	}

	IEnumerator EatingTimer(){
		yield return new WaitForSeconds(6.0f);
		attentionSpan = 6.0f;
		state = CustomerStates.WaitForCheck;
		StartCoroutine(SatisfactionTimer());
	}

	public void NotifyLeave(){
		RestaurantManager.Instance.RemoveElement(customerID);
	}

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
