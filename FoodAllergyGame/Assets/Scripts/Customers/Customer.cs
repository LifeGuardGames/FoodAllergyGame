using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour, IWaiterSelection{
	// ID an id given on creation
	// Select food keyword based off allergen and random rolls
	// Allergen random between wheat, dairy and peanut
	// State the current state of the customer
	// NotifyLeave gives the user money based of satifation and then passes the id to the dayManager

	public int tableNum;

	public float timer = 1.0f;

	// The customer's id used for identification in the 
	public string customerID;

	// The current state of the customer
	public CustomerStates state;

	// The allergy of the customer
	public Allergies allergy;

	// Time spent looking at the menu
	private float menuTimer = 4.0f;

	// The attention timer
	private float attentionSpan = 10.0f;

	// The satisfaction the customer has, everytime the attention span ticks down to 0 the customer will lose satisfaction
	public int satisfaction;

	public FoodKeywords desiredFood;

	private GameObject table;

	public GameObject order;

	public CustomerUIController customerUI;
	public CustomerAnimController customerAnim;
	public GameObject TempOrder;

	public List <ImmutableDataFood> choices;

	// Basic intitialzation
	public virtual void Init(int num, ImmutableDataEvents mode){
		customerUI.ToggleWait(false);
		customerUI.ToggleStar(false);
		customerUI.ToggleAllergyAttack(false);
		customerUI.ToggleText(false, "");

		customerID = "Customer" + num.ToString();
		gameObject.name = "Customer" + num.ToString();
		state = CustomerStates.InLine;
		StartCoroutine("SatisfactionTimer");
		choices = new List<ImmutableDataFood>();
		//allergy = new List<Allergies>();
		satisfaction = 3;

		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);

		switch(mode.CustomerMod){
		case "0":
			timer = 1;
			break;
		case "1":
			timer = 2;
			break;
		case "2":
			timer = 0.8f;
			break;
		}
		attentionSpan = 10f * timer;
		if(GameObject.Find("Line").GetComponent<LineController>().NewCustomer() == null){
			Destroy (this.gameObject);
		}
		else{
			this.gameObject.transform.SetParent(GameObject.Find("Line").GetComponent<LineController>().NewCustomer());
			this.gameObject.transform.position = transform.parent.position;
			SelectAllergy(mode.Allergy);
			int rand = Random.Range(0,3);
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

	private void SelectAllergy(string mode){
		if(mode == "None"){
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
		}
		else if (mode == "Peanut"){
			int rand = Random.Range (0,10);
			if(rand < 7){
				allergy = Allergies.Peanut;			
				}
			else if (rand == 7){
				allergy = Allergies.None;
			}
			else if (rand == 8){
				allergy = Allergies.Wheat;
			}
			else if (rand == 9){
				allergy = Allergies.Dairy;
			}
		}
		else if (mode == "Dairt"){
			int rand = Random.Range (0,10);
			if(rand < 7){
				allergy = Allergies.Dairy;			
			}
			else if (rand == 7){
				allergy = Allergies.None;
			}
			else if (rand == 8){
				allergy = Allergies.Wheat;
			}
			else if (rand == 9){
				allergy = Allergies.Peanut;
			}
		}
		else if (mode == "Wheat"){
			int rand = Random.Range (0,10);
			if(rand < 7){
				allergy = Allergies.Wheat;			
			}
			else if (rand == 7){
				allergy = Allergies.None;
			}
			else if (rand == 8){
				allergy = Allergies.Peanut;
			}
			else if (rand == 9){
				allergy = Allergies.Dairy;
			}
		}
	}


	// When completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		yield return new WaitForSeconds(attentionSpan);
		Debug.Log("wait for seconds done " + gameObject.name);
		if(satisfaction > 0){
			satisfaction--;
			customerUI.UpdateSatisfaction(satisfaction);
			customerAnim.SetSatisfaction(satisfaction);
		}
		if(satisfaction == 0){
			if(order != null){
				Destroy(order.gameObject);
			}
			NotifyLeave();
		}
		StartCoroutine("SatisfactionTimer");
	}

	// JumpToTable jumps to the table given a table number
	public virtual void JumpToTable(int tableN){
		Waiter.Instance.currentLineCustomer = null;
		tableNum = tableN;
		table = RestaurantManager.Instance.GetTable(tableN);
		transform.SetParent(table.transform.GetChild(1));
		transform.localPosition = Vector3.zero;
		state = CustomerStates.ReadingMenu;
		StartCoroutine("ReadMenu");

		customerAnim.SetReadingMenu(true);

		StopCoroutine("SatisfactionTimer");
		attentionSpan = 20.0f * timer;
		StartCoroutine("SatisfactionTimer");
		GetComponentInParent<Table>().currentCustomerID = customerID;
	}

	// Time spent reading menu before ordering
	IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);
		customerAnim.SetReadingMenu(false);
		choices = FoodManager.Instance.GetMenuFoodsFromKeyword(desiredFood);
		customerUI.ToggleWait(true);
		StopCoroutine("SatisfactionTimer");
		attentionSpan = 16.0f *timer;
		StartCoroutine("SatisfactionTimer");
		state = CustomerStates.WaitForOrder;
		//TODO show customer waiting for order
	}

	// Gives the order to the waiter
	public virtual void GetOrder(){
		//TODO return the supplied order
		//TODO display table number on table
		if(Waiter.Instance.CheckHands()){
			Waiter.Instance.canMove = false;
			customerUI.ToggleText(true, allergy.ToString());
			GameObject.Find("MenuUIManager").GetComponent<MenuUIManager>().ShowChoices(choices, tableNum);
		}
	}

	public virtual void OrderTaken(ImmutableDataFood food){
		customerUI.ToggleWait(false);
		customerUI.ToggleText(false, "");
		GameObject orderObj = GameObjectUtils.AddChildWithPositionAndScale(null, TempOrder);
		orderObj.GetComponent<Order>().Init(food.ID, tableNum, food.AllergyList[0]);
		RestaurantManager.Instance.GetTable(tableNum).GetComponent<Table>().OrderObtained(orderObj);
		Waiter.Instance.canMove = true;
		attentionSpan = 16.0f * timer;
		state = CustomerStates.WaitForFood;
		StopCoroutine("SatisfactionTimer");
		satisfaction++;

		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);

		StartCoroutine("SatisfactionTimer");
	}

	// Tells the waiter the food has been delivered and begins eating
	public virtual void Eating(){
		satisfaction++;

		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		customerAnim.SetEating(true);

		order = transform.GetComponentInParent<Table>().FoodDelivered();
		order.GetComponent<BoxCollider>().enabled = false;
		StopCoroutine("SatisfactionTimer");
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
		customerUI.ToggleStar(true);
		customerAnim.SetEating(false);
		attentionSpan = 16.0f*timer;
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		state = CustomerStates.WaitForCheck;
		StartCoroutine("SatisfactionTimer");
	}

	// Tells the resturantManager that the customer is leaving and can be removed from the dictionary
	public virtual void NotifyLeave(){
		RestaurantManager.Instance.CustomerLeft(customerID, satisfaction);
		if(state != CustomerStates.InLine){
			table.GetComponent<Table>().CustomerLeaving();
		}
		Destroy(this.gameObject);
	}

	public virtual void AllergyAttack(){
//		attentionSpan = 5.0f;
		customerUI.ToggleAllergyAttack(true);
		RestaurantManager.Instance.SickCustomers.Add(this.gameObject);
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		StartCoroutine("AllergyTimer");
	}

	public virtual void Saved(){
		RestaurantManager.Instance.SickCustomers.Remove(this.gameObject);
		satisfaction++;
		customerUI.ToggleAllergyAttack(false);
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		customerUI.ToggleStar(true);
		state = CustomerStates.WaitForCheck;
		StopCoroutine("AllergyTimer");
	}

	IEnumerator AllergyTimer(){
		yield return new WaitForSeconds(5.0f);
		RestaurantManager.Instance.SickCustomers.Remove(this.gameObject);
		satisfaction = -5;
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);

		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		NotifyLeave();
	}

	// Checks the current state and runs the appropriate function called by table when waiter approaches
	public virtual void CheckState(){
		switch(state){
		case CustomerStates.WaitForOrder:
			GetOrder();
			break;
		case CustomerStates.WaitForFood:
			if(Waiter.Instance.hand1 == WaiterHands.Meal || Waiter.Instance.hand2 == WaiterHands.Meal){
				if(Waiter.Instance.HaveMeal(tableNum)){
					Eating();
				}
			}
			break;
		case CustomerStates.WaitForCheck:
			NotifyLeave();
			break;
		default:
			break;
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		// Dont do anything, table will talk to customer
		Waiter.Instance.Finished();
	}

	public void OnClicked(){
		if(state == CustomerStates.InLine){
			// If you were already selecting a customer, untween that
			if(Waiter.Instance.currentLineCustomer != null){
				Customer otherCustomerScript = Waiter.Instance.currentLineCustomer.GetComponent<Customer>();
				if(otherCustomerScript != null){
					if(otherCustomerScript.state == CustomerStates.InLine){
						otherCustomerScript.transform.localScale = Vector3.one;
					}
				}
			}
			Waiter.Instance.currentLineCustomer = gameObject;
			gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		}
	}
	#endregion
}
