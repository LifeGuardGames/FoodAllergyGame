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
	public CustomerTypes type = CustomerTypes.Normal;
	public string customerID;			// The customer's id used for identification in the 
	public CustomerStates state;		// The current state of the customer
	public Allergies allergy;			// The allergy of the customer
	private float menuTimer = 4.0f;		// Time spent looking at the menu
	private float attentionSpan = 10.0f;// The attention timer

	public int satisfaction;			// The satisfaction the customer has, everytime the attention span 
										//	ticks down to 0 the customer will lose satisfaction

	public FoodKeywords desiredFood;	// The keyword to help foodmanager find what the customer wants

	public GameObject order;			//the order created by the customer used to have easy access to the
										//	order should the customer leave before they eat it

	public CustomerUIController customerUI;		// ui controller for the customers handles hearts, timer icon and death icon
	public CustomerAnimController customerAnim;	// handles animations
	public GameObject TempOrder;				// temp variable used for instatiation
	public List <ImmutableDataFood> choices;	// a list containing possible options the user would like to eat

	// Basic intitialzation
	public virtual void Init(int num, ImmutableDataEvents mode){
		AudioManager.Instance.PlayClip("customerEnter");

		customerUI.ToggleWait(false);
		customerUI.ToggleStar(false);
		customerUI.ToggleAllergyAttack(false);
//		customerUI.ToggleAllergyShow(false, Allergies.None); TODO safe to remove

		// simple tempoary naming convention
		customerID = "Customer" + num.ToString();
		gameObject.name = "Customer" + num.ToString();
		// customer starts inline
		state = CustomerStates.InLine;
		// star the timer which is used to measure the customer's hearts
		StartCoroutine("SatisfactionTimer");
		//init list
		choices = new List<ImmutableDataFood>();
		//allergy = new List<Allergies>();
		//init satisfaction and update the ui
		satisfaction = 3;
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		// used for events this switches the timer variable which directly affects the customer's satisfaction
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
		//calculates the initial attentionSpan
		attentionSpan = 10f * timer;
		// customers refuse to line up out the door
		if(RestaurantManager.Instance.GetLine().NewCustomer() == null){
			Destroy(this.gameObject);
		}
		else{
			this.gameObject.transform.SetParent(RestaurantManager.Instance.GetLine().NewCustomer());
			this.gameObject.transform.position = transform.parent.position;
			// choose allergy based on the event
			SelectAllergy(mode.Allergy);
			// gets the keyword to help narrow down the customer's choices
			int rand = Random.Range(0, 3);
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
	// chooses an allergy cases where  specific allergy is noted we use a weighted random to get the desired result
	private void SelectAllergy(string mode){
		if(mode == "None"){
			int rand = Random.Range(0, 4);
			switch(rand){
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
		else if(mode == "Peanut"){
			int rand = Random.Range(0, 10);
			if(rand < 7){
				allergy = Allergies.Peanut;			
			}
			else if(rand == 7){
				allergy = Allergies.None;
			}
			else if(rand == 8){
				allergy = Allergies.Wheat;
			}
			else if(rand == 9){
				allergy = Allergies.Dairy;
			}
		}
		else if(mode == "Dairy"){
			int rand = Random.Range(0, 10);
			if(rand < 7){
				allergy = Allergies.Dairy;			
			}
			else if(rand == 7){
				allergy = Allergies.None;
			}
			else if(rand == 8){
				allergy = Allergies.Wheat;
			}
			else if(rand == 9){
				allergy = Allergies.Peanut;
			}
		}
		else if(mode == "Wheat"){
			int rand = Random.Range(0, 10);
			if(rand < 7){
				allergy = Allergies.Wheat;			
			}
			else if(rand == 7){
				allergy = Allergies.None;
			}
			else if(rand == 8){
				allergy = Allergies.Peanut;
			}
			else if(rand == 9){
				allergy = Allergies.Dairy;
			}
		}

	}


	// When completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		yield return new WaitForSeconds(attentionSpan);
//		Debug.Log("wait for seconds done " + gameObject.name);
		if(satisfaction > 0){
			satisfaction--;
			customerUI.UpdateSatisfaction(satisfaction);
			customerAnim.SetSatisfaction(satisfaction);
		}
		// then if satisfaction is 0 we need to leave cause the service is rubbish
		if(satisfaction == 0){
			if(order != null){
				Destroy(order.gameObject);
			}
			NotifyLeave();
		}
		// if we still have satisfaction left we start the timer again
		StartCoroutine("SatisfactionTimer");
	}

	// Jumps to the table given a table number
	public virtual void JumpToTable(int tableN){
		Waiter.Instance.currentLineCustomer = null;
		AudioManager.Instance.PlayClip("pop");
		//sitting down
		tableNum = tableN;
		transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).Seat);
		transform.localPosition = Vector3.zero;
		// begin reading menu
		state = CustomerStates.ReadingMenu;
		StartCoroutine("ReadMenu");
		AudioManager.Instance.PlayClip("readingMenu");
		StopCoroutine("SatisfactionTimer");
		customerAnim.SetReadingMenu(true);
		GetComponentInParent<Table>().currentCustomerID = customerID;
		this.GetComponent<SphereCollider>().enabled = false;
	}

	// Time spent reading menu before ordering
		IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);

		customerAnim.SetReadingMenu(false);
		//get food choices 
		choices = FoodManager.Instance.GetMenuFoodsFromKeyword(desiredFood);
		customerUI.ToggleWait(true);
		//stop the satisfaction timer, change the timer and then restart it
		attentionSpan = 16.0f * timer;
		StartCoroutine("SatisfactionTimer");
		// now waiting for our order to be taken
		state = CustomerStates.WaitForOrder;
		AudioManager.Instance.PlayClip("orderTime");
	}

	// Gives the order to the waiter
	public virtual void GetOrder(){
		if(state == CustomerStates.WaitForOrder){
			// check to see if we have an open hand for the order
			if(Waiter.Instance.CheckHands()){
				StopCoroutine("SatisfactionTimer");
				// lock our waiter 
				Waiter.Instance.canMove = false;
				RestaurantManager.Instance.GetMenuUiManager().ShowChoices(choices, tableNum, allergy);
			}
		}
		else{
			Waiter.Instance.Finished();
		}
	}
	// It's called when the button for food is hit get the customer to make his order and hand it to the waiter
	public virtual void OrderTaken(ImmutableDataFood food){
		if(order == null){
			order = TempOrder;
			customerUI.ToggleWait(false);
			GameObject orderObj = GameObjectUtils.AddChildWithPositionAndScale(null, TempOrder);
			orderObj.GetComponent<Order>().Init(food.ID, tableNum, food.AllergyList[0]);
			RestaurantManager.Instance.GetTable(tableNum).GetComponent<Table>().OrderObtained(orderObj);
			state = CustomerStates.WaitForFood;
			Waiter.Instance.Finished();
			attentionSpan = 20.0f * timer;
			//StopCoroutine("SatisfactionTimer");
			IncreaseSatisfaction();

			customerUI.UpdateSatisfaction(satisfaction);
			customerAnim.SetSatisfaction(satisfaction);

			StartCoroutine("SatisfactionTimer");
		}
	}

	// Tells the waiter the food has been delivered and begins eating
	public virtual void Eating(){
		IncreaseSatisfaction();

		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		customerAnim.SetEating(true);

		order = transform.GetComponentInParent<Table>().FoodDelivered();
		order.GetComponent<BoxCollider>().enabled = false;
		order.GetComponentInChildren<UnityEngine.UI.Text>().text = "";
		StopCoroutine("SatisfactionTimer");
		if(order.GetComponent<Order>().allergy == allergy && allergy != Allergies.None){
			state = CustomerStates.AllergyAttack;
			AllergyAttack();
		}
		else{
			state = CustomerStates.Eating;
			StartCoroutine("EatingTimer");
			AudioManager.Instance.PlayClip("eating");
		}
	}

	// Eating coroutine
		IEnumerator EatingTimer(){
		yield return new WaitForSeconds(6.0f);
		customerUI.ToggleStar(true);
		customerAnim.SetEating(false);
		attentionSpan = 10.0f * timer;
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		state = CustomerStates.WaitForCheck;
		StartCoroutine("SatisfactionTimer");
		AudioManager.Instance.PlayClip("readyForCheck");
	}

	// Tells the resturantManager that the customer is leaving and can be removed from the dictionary
	public virtual void NotifyLeave(){
		RestaurantManager.Instance.CustomerLeft(customerID, satisfaction);
		if(state != CustomerStates.InLine){
			RestaurantManager.Instance.GetTable(tableNum).CustomerLeaving();
		}
		AudioManager.Instance.PlayClip("leaving");
		Destroy(this.gameObject);
	}
	// called if the food's ingrediants match the allergy starts a timer in which the player must hit the save me button
	public virtual void AllergyAttack(){
//		attentionSpan = 5.0f;
		if(RestaurantManager.Instance.firstSickCustomer){
			RestaurantManager.Instance.firstSickCustomer = false;
			RestaurantManager.Instance.medicButton.GetComponent<Animator>().SetBool("TutMedic", true);
			RestaurantManager.Instance.tutText.SetActive(true);
		}
		customerUI.ToggleAllergyAttack(true);
		RestaurantManager.Instance.SickCustomers.Add(this.gameObject);
		AudioManager.Instance.PlayClip("allergyAttack");
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		StartCoroutine("AllergyTimer");
	}
	// if they are saved they take a small penalty for making the mistake and the customer will want the check asap
	public virtual void Saved(){
		RestaurantManager.Instance.medicButton.GetComponent<Animator>().SetBool("TutMedic", false);
		RestaurantManager.Instance.tutText.SetActive(false);
		RestaurantManager.Instance.UpdateCash(-10f);
		RestaurantManager.Instance.SickCustomers.Remove(this.gameObject);
		IncreaseSatisfaction();
		customerAnim.SetSatisfaction(satisfaction);
		customerUI.ToggleAllergyAttack(false);
		customerUI.UpdateSatisfaction(satisfaction);
		customerUI.ToggleStar(true);
		state = CustomerStates.WaitForCheck;
		StopCoroutine("AllergyTimer");
	}
	// when it runs out the customer is taken to the hospital and the player is slamed with the bill
	IEnumerator AllergyTimer(){
		yield return new WaitForSeconds(5.0f);
		RestaurantManager.Instance.medicButton.GetComponent<Animator>().SetBool("TutMedic", false);
		RestaurantManager.Instance.tutText.SetActive(false);
		RestaurantManager.Instance.SickCustomers.Remove(this.gameObject);
		satisfaction = -10;
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		AudioManager.Instance.PlayClip("dead");
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		NotifyLeave();
	}

	// Checks the current state and runs the appropriate function called by table when waiter approaches
	public virtual void CheckState(){
		switch(state){
		case CustomerStates.WaitForOrder:
			if(Waiter.Instance.hand1 != WaiterHands.None && Waiter.Instance.hand2 != WaiterHands.None){
				Waiter.Instance.Finished();
				break;
			}
			GetOrder();
			break;
		case CustomerStates.WaitForFood:
			if(Waiter.Instance.hand1 == WaiterHands.Meal || Waiter.Instance.hand2 == WaiterHands.Meal){
				if(Waiter.Instance.HaveMeal(tableNum)){
					Eating();
				}
				else{
					Waiter.Instance.Finished();
				}
			}
			else{
				Waiter.Instance.Finished();
			}
			break;
		case CustomerStates.WaitForCheck:
			NotifyLeave();
			break;
		default:
			Waiter.Instance.Finished();
			break;
		}
	}

	public void IncreaseSatisfaction(){
		if(satisfaction < 3){
			satisfaction++;
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
			AudioManager.Instance.PlayClip("pop");
			gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		}
	}
	#endregion

	public virtual void Playing(){
		this.transform.GetChild(0).gameObject.SetActive(false);
		this.transform.GetChild(1).gameObject.SetActive(false);
		StopCoroutine("SatisfactionTimer");
		StartCoroutine("PlayTime");
	}

	IEnumerator PlayTime(){
		yield return new WaitForSeconds(10.0f);
		this.transform.GetChild(0).gameObject.SetActive(true);
		this.transform.GetChild(1).gameObject.SetActive(true);
		StartCoroutine("SatisfactionTimer");
	}
}
