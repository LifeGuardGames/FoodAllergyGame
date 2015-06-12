using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour{
	// ID an id given on creation
	// Select food keyword based off allergen and random rolls
	// Allergen random between wheat, dairy and peanut
	// State the current state of the customer
	// NotifyLeave gives the user money based of satifation and then passes the id to the dayManager

	public int tableNum;

	// The customer's id used for identification in the 
	public string customerID;

	// The current state of the customer
	public CustomerStates state;

	// The allergy of the customer
	public Allergies allergy;

	// Time spent looking at the menu
	private float menuTimer = 4.0f;

	// The attention timer
	private float attentionSpan = 10f;

	// The satisfaction the customer has, everytime the attention span ticks down to 0 the customer will lose satisfaction
	public int satisfaction;

	public FoodKeywords desiredFood;

	private GameObject table;

	public GameObject order;

	public GameObject TempOrder;

	public GameObject exclaimationMark;

	public List <ImmutableDataFood> choices;

	// Basic intitialzation
	public void Init(int num){
		exclaimationMark = transform.GetChild(0).gameObject;
		exclaimationMark.SetActive(false);
		customerID = "Customer" + num.ToString();
		state = CustomerStates.InLine;
		StartCoroutine(SatisfactionTimer());
		choices = new List<ImmutableDataFood>();
		//allergy = new List<Allergies>();
		satisfaction = 3;
		if(GameObject.Find("Line").GetComponent<LineController>().NewCustomer() == null){
			Destroy (this.gameObject);
		}
		else{
			this.gameObject.transform.SetParent(GameObject.Find("Line").GetComponent<LineController>().NewCustomer());
			this.gameObject.transform.position = transform.parent.position;
			int rand = Random.Range (0,4);
			switch (rand){
			case 0:
				allergy = Allergies.Dairy;
				break;
			case 1:
				allergy = Allergies.Peanut;
				break;
			case 2:
				allergy = Allergies.Wheat;
				break;
			case 3:
				allergy = Allergies.None;
				break;
			}
			rand = Random.Range(0,3);
			switch(rand){
			case 0:
				desiredFood = FoodKeywords.Meal;
				break;
			case 1:
				desiredFood = FoodKeywords.Drink;
				break;
			case 2:
				desiredFood = FoodKeywords.Dessert;
				break;
//			case 3:
//				desiredFood = FoodKeywords.Drink;
//				break;
//			case 4:
//				desiredFood = FoodKeywords.Green;
//				break;
//			case 5:
//				desiredFood = FoodKeywords.Meat;
//				break;
//			case 6:
//				desiredFood = FoodKeywords.Nut;
//				break;
			}
		}
	}

	// When completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		yield return new WaitForSeconds(attentionSpan);
		if(satisfaction >0){
			satisfaction--;
		}
		StartCoroutine(SatisfactionTimer());
	}

	// JumpToTable jumps to the table given a table number
	public void JumpToTable(int tableN){
		Waiter.Instance.currentlyServing = null;
		tableNum = tableN;
		table = RestaurantManager.Instance.GetTable(tableN);
		transform.SetParent(table.transform.GetChild(1));
		transform.localPosition = Vector3.zero;
		state = CustomerStates.ReadingMenu;
		StartCoroutine("ReadMenu");
		StopCoroutine(SatisfactionTimer());
		attentionSpan = 20.0f;
		StartCoroutine(SatisfactionTimer());
	}

	// Time spent reading menu before ordering
	IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);

		choices = FoodManager.Instance.GetMenuFoodsFromKeyword(desiredFood);
		exclaimationMark.SetActive(true);
		StopCoroutine(SatisfactionTimer());
		attentionSpan = 16.0f;
		StartCoroutine(SatisfactionTimer());
		state = CustomerStates.WaitForOrder;
		//TODO show customer waiting for order
	}

	// Gives the order to the waiter
	public void GetOrder(){
		//TODO return the supplied order
		//TODO display table number on table
		if(Waiter.Instance.CheckHands()){
			GameObject.Find("MenuUIManager").GetComponent<MenuUIManager>().ShowChoices(choices, tableNum);
		}
	}

	public void OrderTaken(ImmutableDataFood food){
		exclaimationMark.SetActive(false);
		GameObject orderObj = Instantiate(TempOrder,new Vector3 (0,0,0), TempOrder.transform.rotation)as GameObject;
		orderObj.GetComponent<Order>().Init(food.ID,tableNum,food.AllergyList[0]);
		RestaurantManager.Instance.GetTable(tableNum).GetComponent<Table>().OrderObtained(orderObj);
		attentionSpan = 16.0f;
		state = CustomerStates.WaitForFood;
		StopCoroutine(SatisfactionTimer());
		satisfaction++;
		StartCoroutine(SatisfactionTimer());
	}

	// Tells the waiter the food has been delivered and begins eating
	public void Eating(){
		satisfaction++;
		order = transform.GetComponentInParent<Table>().FoodDelivered();
		order.GetComponent<BoxCollider>().enabled = false;
		StopCoroutine(SatisfactionTimer());
		if(order.GetComponent<Order>().allergy == allergy){
			state = CustomerStates.AllergyAttack;
			AllergyAttack();
		}
		else{
		state = CustomerStates.Eating;
		StartCoroutine("EatingTimer");
		}
	}

	// Eating coroutine
	IEnumerator EatingTimer(){
		yield return new WaitForSeconds(6.0f);
		exclaimationMark.SetActive(true);
		attentionSpan = 16.0f;
		Destroy(order.gameObject);
		state = CustomerStates.WaitForCheck;
		StartCoroutine(SatisfactionTimer());
	}

	// Tells the resturantManager that the customer is leaving and can be removed from the dictionary
	public void NotifyLeave(){
		RestaurantManager.Instance.CustomerLeft(customerID, satisfaction);
		table.GetComponent<Table>().inUse = false;
		Destroy(this.gameObject);
	}

	public void AllergyAttack(){
//		attentionSpan = 5.0f;
		exclaimationMark.SetActive(true);
		RestaurantManager.Instance.SickCustomers.Add (this.gameObject);
		StartCoroutine("AllergyTimer");
	}

	public void Saved(){
		RestaurantManager.Instance.SickCustomers.Remove(this.gameObject);
		satisfaction++;
		StopCoroutine("AllergyTimer");
	}

	IEnumerator AllergyTimer(){
		yield return new WaitForSeconds(5.0f);
		RestaurantManager.Instance.SickCustomers.Remove(this.gameObject);
		satisfaction = -5;
		Destroy(order.gameObject);
		NotifyLeave();
	}

	// Checks the current state and runs the appropriate function called by table when waiter approaches
	public void CheckState(){
		switch(state){
		case CustomerStates.WaitForOrder:
			GetOrder();
			break;
		case CustomerStates.WaitForFood:
			if(Waiter.Instance.hand1 == WaiterHands.Meal || Waiter.Instance.hand2 == WaiterHands.Meal){
				Eating();
			}
			break;
		case CustomerStates.WaitForCheck:
			NotifyLeave();
			break;
		default:
			break;
		}
	}
}
