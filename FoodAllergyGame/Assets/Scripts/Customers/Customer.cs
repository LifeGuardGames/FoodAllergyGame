using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Customer AI is now handled through a series of classes known as behav which are childrens of the customer component generic class
/// The customer class now only handles interactions between a customer and an outside object such as waiter and order
/// for more information on behavs check out customer Component
/// </summary>
public class Customer : MonoBehaviour, IWaiterSelection{

	public int tableNum = -1;
	public float timer = 1.0f;
	public CustomerTypes type = CustomerTypes.Normal;
	public string customerID;           // The customer's id used for identification in the 
	public string customerIDMissionKey;
	public CustomerStates state;		// The current state of the customer
	public List<Allergies> allergy;		// The allergy of the customer
	public float menuTimer = 4.0f;		// Time spent looking at the menu
	public float attentionSpan = 15.0f;	// The attention timer
	public float eatTimer = 6.0f;
	public Behav currBehav;
	public int satisfaction;            // The satisfaction the customer has, everytime the attention span 
										//	ticks down to 0 the customer will lose satisfaction

	public bool failedMission;
	public bool completedMission;
	public FoodKeywords desiredFood;    // The keyword to help foodmanager find what the customer wants

	private GameObject order;           //the order created by the customer used to have easy access to the
										//	order should the customer leave before they eat it
	public GameObject Order {
		get { return order; }
		set { order = value; }
	}

	public CustomerUIController customerUI;     // ui controller for the customers handles hearts, timer icon and death icon
	public MeshRenderer customerMeshRenderer;
	public CustomerAnimController customerAnim;	// handles animations
	public GameObject OrderPrefab;				// temp variable used for instatiation
	public List <ImmutableDataFood> choices;	// a list containing possible options the user would like to eat
	public bool hasPowerUp;
	public int priceMultiplier;
	private int playAreaIndexAux = -1;				// For use in coroutine, needs parameter with string call
    public float spawnTime;
	public bool saved = false;
	public string behavFlow;
	public bool sneakOut = false;
	public bool ordered = false;
	public bool isAnnoyed = false;

	// Basic intitialzation
	public virtual void Init(int num, ImmutableDataEvents mode){
		spawnTime = Time.time;
        AudioManager.Instance.PlayClip("CustomerEnter");

		customerUI.ToggleWait(false);
		customerUI.ToggleStar(false);
		customerUI.ToggleAllergyAttack(false);

		// simple temporary naming convention
		customerID = "Customer" + num.ToString();
		gameObject.name = "Customer" + num.ToString();
		// customer starts inline
		state = CustomerStates.InLine;
		// star the timer which is used to measure the customer's hearts
	
		//init list
		choices = new List<ImmutableDataFood>();
		//allergy = new List<Allergies>();
		//init satisfaction and update the ui
		satisfaction = 3;
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetWaitingInLine();
		// used for events this switches the timer variable which directly affects the customer's satisfaction
		timer = mode.CustomerTimerMod;
		//calculates the initial attentionSpan
		attentionSpan = 15f * timer;
		menuTimer *= RandomFactor();
		eatTimer *= RandomFactor();
		behavFlow = DataLoaderBehav.GetRandomBehavByType(type.ToString()).ID;
		StartCoroutine("SatisfactionTimer");

		// customers refuse to line up out the door
		if(!RestaurantManager.Instance.LineController.HasEmptySpot()){
			Destroy(this.gameObject);
		}
		else{
			// Check for fly thru table
			TableFlyThru flyThruTable = RestaurantManager.Instance.GetFlyThruTable();
			if((flyThruTable != null) && UnityEngine.Random.Range(0,10) > 3 && !flyThruTable.inUse){
				flyThruTable.inUse = true;
				transform.SetParent(flyThruTable.seat);
				transform.localPosition = Vector3.zero;
				tableNum = flyThruTable.TableNumber;
				//state = CustomerStates.ReadingMenu;
				//StartCoroutine("ReadMenu");
				//StopCoroutine("SatisfactionTimer");
				customerAnim.SetReadingMenu();
				GetComponentInParent<Table>().currentCustomerID = customerID;
				this.GetComponent<BoxCollider>().enabled = false;
				flyThruTable.FlyThruDropDown();
				var _type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[1]);
				Behav read = (Behav)Activator.CreateInstance(_type);
				read.self = this;
				currBehav = read;
				read.Act();
				//BehavReadingMenu read = new BehavReadingMenu(self);
				read = null;
			}
			else{
				RestaurantManager.Instance.restaurantUI.OpenAndCloseDoor();
				RestaurantManager.Instance.LineController.PlaceCustomerInEmptySpot(this);   // Set customer line spot and update base sorting order
				RestaurantManager.Instance.lineCount++;
				var _type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[0]);
				Behav wait = (Behav)Activator.CreateInstance(_type);
				wait.self = this;
				currBehav = wait;
				wait.Act();
				wait = null;
			}
			this.transform.position = transform.parent.position;	
		}

		// choose allergy based on the event
		SelectAllergy(mode.Allergy);
		// gets the keyword to help narrow down the customer's choices
		int rand = UnityEngine.Random.Range(0, 3);
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
		}
	}

	// Basic intitialzation
	public virtual void Init(int num, ImmutableDataChallenge mode) {
		spawnTime = Time.time;
		AudioManager.Instance.PlayClip("CustomerEnter");

		customerUI.ToggleWait(false);
		customerUI.ToggleStar(false);
		customerUI.ToggleAllergyAttack(false);

		// simple temporary naming convention
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
		customerAnim.SetWaitingInLine();
		// used for events this switches the timer variable which directly affects the customer's satisfaction
		timer = mode.CustomerTimerMod;
		//calculates the initial attentionSpan
		attentionSpan = 15f * timer * RandomFactor();

		// customers refuse to line up out the door
		if(!RestaurantManager.Instance.LineController.HasEmptySpot()) {
			Destroy(this.gameObject);
		}
		else {
			// Check for fly thru table
			TableFlyThru flyThruTable = RestaurantManager.Instance.GetFlyThruTable();
			if((flyThruTable != null) && UnityEngine.Random.Range(0, 10) > 3 && !flyThruTable.inUse && !RestaurantManager.Instance.isTutorial || mode.ID == "TutDecoFlyThru") {
				flyThruTable.inUse = true;
				transform.SetParent(flyThruTable.seat);
				transform.localPosition = Vector3.zero;
				tableNum = flyThruTable.TableNumber;
				state = CustomerStates.ReadingMenu;
				//StartCoroutine("ReadMenu");
				//StopCoroutine("SatisfactionTimer");
				customerAnim.SetReadingMenu();
				GetComponentInParent<Table>().currentCustomerID = customerID;
				this.GetComponent<BoxCollider>().enabled = false;
				flyThruTable.FlyThruDropDown();
				var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[1]);
				Behav read = (Behav)Activator.CreateInstance(type);
				read.self = this;
				currBehav = read;
				read.Act();
				//BehavReadingMenu read = new BehavReadingMenu(self);
				read = null;
			}
			else {
				RestaurantManager.Instance.restaurantUI.OpenAndCloseDoor();
				RestaurantManager.Instance.LineController.PlaceCustomerInEmptySpot(this);   // Set customer line spot and update base sorting order
				RestaurantManager.Instance.lineCount++;
				this.transform.position = transform.parent.position;
				var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[0]);
				Behav wait = (Behav)Activator.CreateInstance(type);
				currBehav = wait;
				wait.self = this;
				wait.Act();
				wait = null;
			}	
		}

		// choose allergy based on the event
		SelectAllergy(mode.Allergy);
		// gets the keyword to help narrow down the customer's choices
		int rand = UnityEngine.Random.Range(0, 3);
		switch(rand) {
			case 0:
				desiredFood = FoodKeywords.Meal;
				break;
			case 1:
				desiredFood = FoodKeywords.Drink;
				break;
			case 2:
				desiredFood = FoodKeywords.Dessert;
				break;
		}
	}

	/// <summary>
	/// Base sorting order from the node placed itself
	/// If customer mesh order is B, customer UI canvas is set to B+5, table will be set in the middle B+1 and B+2
	/// </summary>
	public void SetBaseSortingOrder(int baseSortingOrder) {
		customerMeshRenderer.sortingOrder = baseSortingOrder;
		customerUI.UpdateSortingOrder(baseSortingOrder + 5);
	}

	// chooses an allergy cases where specific allergy is noted we use a weighted random to get the desired result
	private void SelectAllergy(string mode){
		if(mode == "None"){
			int rand = UnityEngine.Random.Range(0, 4);
			switch(rand){
			case 0:
				allergy.Add(Allergies.Dairy);
				break;
			case 1:
				allergy.Add(Allergies.Peanut);
				break;
			case 2:
				allergy.Add(Allergies.Wheat);
				break;
			case 3:
				allergy.Add(Allergies.None);
				break;
			}
		}
		else if(mode == "Peanut"){
			int rand = UnityEngine.Random.Range(0, 10);
			if(rand < 7){
				allergy.Add(Allergies.Peanut);			
			}
			else if(rand == 7){
				allergy.Add(Allergies.None);
			}
			else if(rand == 8){
				allergy.Add(Allergies.Wheat);
			}
			else if(rand == 9){
				allergy.Add(Allergies.Dairy);
			}
		}
		else if(mode == "Dairy"){
			int rand = UnityEngine.Random.Range(0, 10);
			if(rand < 7){
				allergy.Add(Allergies.Dairy);			
			}
			else if(rand == 7){
				allergy.Add(Allergies.None);
			}
			else if(rand == 8){
				allergy.Add(Allergies.Wheat);
			}
			else if(rand == 9){
				allergy.Add(Allergies.Peanut);
			}
		}
		else if(mode == "Wheat"){
			int rand = UnityEngine.Random.Range(0, 10);
			if(rand < 7){
				allergy.Add(Allergies.Wheat);			
			}
			else if(rand == 7){
				allergy.Add(Allergies.None);
			}
			else if(rand == 8){
				allergy.Add(Allergies.Peanut);
			}
			else if(rand == 9){
				allergy.Add(Allergies.Dairy);
			}
		}
		else if(mode == "cc") {
			allergy.Add(Allergies.None);
		}
	}

	// Note: Not capped
	//TODO Change character animation here???
	public void SetSatisfaction(int _satisfaction){
		satisfaction = _satisfaction;
		customerUI.UpdateSatisfaction(satisfaction);

		// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
		if(satisfaction <= 0){
			if(Order != null){
				Destroy(Order.gameObject);
			}
			BehavNotifyLeave leave = new BehavNotifyLeave();
			leave.self = this;
			leave.Act();
		}
	}

	// Note: Not capped
	public virtual void UpdateSatisfaction(int delta){
		StopCoroutine("BlinkHeart");
		if(tableNum != 4) {
			customerUI.StopLosingHeart(satisfaction);
		}
		// added check incase table 0 is destroyed 
		if(!RestaurantManager.Instance.isTutorial) {
			if(RestaurantManager.Instance.GetTable(tableNum) != null) {

				if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.VIP) {
					if(delta < 0) {
						satisfaction += delta;
						customerUI.UpdateSatisfaction(satisfaction);
						customerAnim.UpdateSatisfaction(delta);

						// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
						if(satisfaction <= 0) {
							if(Order != null) {
								Destroy(Order.gameObject);
							}
							var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[6]);
							Behav leave = (Behav)Activator.CreateInstance(type);
							leave.self = this;
							leave.Act();
						}
					}
				}
				else {
					satisfaction += delta;
					if(satisfaction > 3) {
						satisfaction = 3;
					}
					customerUI.UpdateSatisfaction(satisfaction, true, delta);

					customerAnim.UpdateSatisfaction(delta);

					// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
					if(satisfaction <= 0) {
						if(Order != null) {
							Destroy(Order.gameObject);
						}
						var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[6]);
						Behav leave = (Behav)Activator.CreateInstance(type);
						leave.self = this;
						leave.Act();
					}
				}
			}
			else {
				satisfaction += delta;
				customerUI.UpdateSatisfaction(satisfaction, true, delta);

				customerAnim.UpdateSatisfaction(delta);

				// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
				if(satisfaction <= 0) {
					if(Order != null) {
						Destroy(Order.gameObject);
					}
					var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[6]);
					Behav leave = (Behav)Activator.CreateInstance(type);
					leave.self = this;
					leave.Act();
				}
			}
		}
	}

	// When completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		StopCoroutine("BlinkHeart");
		StartCoroutine("BlinkHeart");
		yield return new WaitForSeconds(attentionSpan);
		UpdateSatisfaction(-1);
		
		// if we still have satisfaction left we start the timer again
		StartCoroutine("SatisfactionTimer");
	}

	IEnumerator BlinkHeart() {
		if(tableNum != 4) {
			customerUI.StopLosingHeart(satisfaction);
		}
		yield return new WaitForSeconds(attentionSpan * 0.5f);
		if(tableNum != 4) {
			customerUI.LosingHeart(satisfaction);
		}
	}


	// Time spent reading menu before ordering
	IEnumerator ReadMenu() {
		yield return new WaitForSeconds(menuTimer);
		if(PlayArea.Instance != null) { 
			if(PlayArea.Instance.cantLeave && playAreaIndexAux != -1) {
				PlayArea.Instance.EndPlayTime(playAreaIndexAux);
			}
		}
		currBehav.Reason();
		if(RestaurantManager.Instance.isTutorial) {
			this.GetComponent<CustomerTutorial>().NextTableFinger();
		}
	}

	// Gives the order to the waiter
	public virtual void GetOrder(){
		if(state == CustomerStates.WaitForOrder){
			if(RestaurantManager.Instance.isTutorial){
				this.GetComponent<CustomerTutorial>().hideTableFinger();
			}
			// check to see if we have an open hand for the order
			if(Waiter.Instance.CheckHands()){
				TouchManager.Instance.PauseQueue();
				StopCoroutine("SatisfactionTimer");
				// lock our waiter
				Waiter.Instance.CanMove = false;
				RestaurantManager.Instance.MenuUIController.ShowChoices(choices, tableNum, allergy);
			}
		}
		else{
			Debug.LogError("Wrong state for customer to get order: " + state.ToString());
			Waiter.Instance.Finished();
		}
	}

	// It's called when the button for food is hit get the customer to make his order and hand it to the waiter
	public virtual void OrderTaken(ImmutableDataFood food){
		if(Order == null){
			customerAnim.SetWaitingForFood();

			customerUI.ToggleWait(false);

			Order = GameObjectUtils.AddChildWithPositionAndScale(null, OrderPrefab);
			Order.GetComponent<Order>().Init(food.ID, tableNum, food.AllergyList);
			if(food == FoodManager.Instance.specialFood) {
				priceMultiplier = food.Reward * 2;
			}
			else if(food == FoodManager.Instance.bannedFood) {
				priceMultiplier = 0;
			}
			else {
				priceMultiplier = food.Reward;
			}
			RestaurantManager.Instance.GetTable(tableNum).GetComponent<Table>().OrderObtained(Order);
			attentionSpan =  KitchenManager.Instance.cookTimer + (20.0f * timer);

			UpdateSatisfaction(1);

			StartCoroutine("SatisfactionTimer");


			// Unpause queue and continue waiter movement
			TouchManager.Instance.UnpauseQueue();
			Waiter.Instance.Finished();
			currBehav.Reason();
		}
		else{
			Debug.LogError("Order already exists: " + Order.name);
			Waiter.Instance.Finished();
		}
	}

	// Tells the waiter the food has been delivered and begins eating
	public virtual void Eating(){
		if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.FlyThru) {
			Waiter.Instance.Finished();
			this.currBehav.Reason();
			for(int i = 0; i < allergy.Count; i++) { 
				if(Order.GetComponent<Order>().allergy.Contains(allergy[i]) && !allergy.Contains(Allergies.None)) {
					Medic.Instance.BillRestaurant(-100);
					ParticleAndFloatyUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(tableNum).gameObject.transform.position, -100);
					RestaurantManager.Instance.GetFlyThruTable().FlyThruLeave();
					AudioManager.Instance.PlayClip("CustomerDead");
					if(Order.gameObject != null) {
						Destroy(Order.gameObject);
					}
					SetSatisfaction(0);
				}
			}
		}
		currBehav.Reason();
	}

	// Eating coroutine
	IEnumerator EatingTimer(){
		yield return new WaitForSeconds(eatTimer);
		currBehav.Reason();
		customerAnim.SetWaitingForCheck();
		if(RestaurantManager.Instance.isTutorial){
			this.GetComponent<CustomerTutorial>().NextTableFinger();
		}
	}
	
	// when it runs out the customer is taken to the hospital and the player is slamed with the bill
	IEnumerator AllergyTimer(){
		yield return new WaitForSeconds(10.0f);
		currBehav.Reason();
	}
	
	// Checks the current state and runs the appropriate function called by table when waiter approaches
	public virtual void CheckState(){
		switch(state){
		case CustomerStates.WaitForOrder:
			if(Waiter.Instance.Hand1 != WaiterHands.None && Waiter.Instance.Hand2 != WaiterHands.None){
				ParticleAndFloatyUtils.PlayHandsFullFloaty(Waiter.Instance.transform.position);
				Waiter.Instance.Finished();
			}
			else{
				GetOrder(); 	// This calls Waiter.Instance.Finished by itself
			}
			break;
		case CustomerStates.WaitForFood:
			if(Waiter.Instance.HaveMeal(tableNum)){
				Eating();
			}
			else{
				Waiter.Instance.Finished();
			}
			break;
		case CustomerStates.WaitForCheck:
			currBehav.Reason();
			//NotifyLeave();
			Waiter.Instance.Finished();
			break;
		default:	// All other states, nothing to do
				if(type == CustomerTypes.CoolKid) {
					//cool kid doesn't like to be disturbed
					UpdateSatisfaction(-1);
					failedMission = true;
				}
			Waiter.Instance.Finished();
			break;
		}
	}


	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		// Dont do anything, table will talk to customer
		Waiter.Instance.Finished();
	}

	public bool IsQueueable(){
		return false;
	}

	public virtual void Deselect(){
		RestaurantManager.Instance.CustomerLineSelectHighlightOff();
		gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		Waiter.Instance.CurrentLineCustomer = null;
	}

	public virtual void OnClicked(){
		if(state == CustomerStates.InLine){
			// If you were already selecting a customer, untween that
			if(Waiter.Instance.CurrentLineCustomer != null){
				if(this.GetComponent<CustomerTutorial>()) {
					this.GetComponent<CustomerTutorial>().hideTableFinger();
                }
				Customer otherCustomerScript = Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>();
				if(otherCustomerScript != null){
					if(otherCustomerScript.state == CustomerStates.InLine){
						otherCustomerScript.transform.localScale = Vector3.one;
					}
				}
			}
			RestaurantManager.Instance.CustomerLineSelectHighlightOn();
			Waiter.Instance.CurrentLineCustomer = gameObject;
			AudioManager.Instance.PlayClip("CustomerSelected");
			gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			if(RestaurantManager.Instance.isTutorial && !this.gameObject.GetComponent<CustomerTutorial>().isAllergy){
				this.GetComponent<CustomerTutorial>().hideTableFinger();
				this.GetComponent<CustomerTutorial>().step = 4;
				this.GetComponent<CustomerTutorial>().nextHint();
			}
			else if(RestaurantManager.Instance.isTutorial && this.gameObject.GetComponent<CustomerTutorial>().isAllergy){
				this.GetComponent<CustomerTutorial>().hideTableFinger();
				this.GetComponent<CustomerTutorial>().step = 6;
				this.GetComponent<CustomerTutorial>().nextHint();
			}
		}
	}

	public virtual void OnPressAnim() {
		// Do nothing
	}

	public void AddQueueUI() {
		// Do nothing
	}

	public void UpdateQueueUI(int order) {
		// Do nothing
	}

	public void DestroyQueueUI() {
		// Do nothing
	}
	#endregion

	public virtual void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		UpdateSatisfaction(deltaSatisfaction);
		RestaurantManager.Instance.PlayAreaUses++;
		transform.position = playAreaSpot;
		GetComponent<BoxCollider>().enabled = false;
		StopCoroutine("SatisfactionTimer");
		Deselect();
		playAreaIndexAux = spotIndex;
		StartCoroutine("PlayTime");
	}

	private IEnumerator PlayTime(){
		yield return new WaitForSeconds(10.0f * PlayArea.Instance.timeMultiplier);
		if(!PlayArea.Instance.cantLeave) {
			// End play
			transform.localPosition = Vector3.zero; // Move the customer back to its position in line
			PlayArea.Instance.EndPlayTime(playAreaIndexAux);
		}
		
		GetComponent<BoxCollider>().enabled = true;
		StartCoroutine("SatisfactionTimer");
	}

	public void DestroySelf(float delay) {
		Destroy(this.gameObject, delay);
	}

	public void DestroyOrder() {
		if(Order != null) {
			Destroy(Order.gameObject);
		}
	}

	public void Reorder() {
		if(RestaurantManager.Instance.trashCanTutorial.activeSelf) {
			RestaurantManager.Instance.trashCanTutorial.SetActive(false);
			DataManager.Instance.GameData.Tutorial.IsTrashCanTutDone = true;
			KitchenManager.Instance.IsTrachcanTut = false;
        }
		DestroyOrder();
		Waiter.Instance.RemoveMeal(tableNum);
		customerUI.ToggleWait(false);
		customerUI.TriggerReorderThought();
		KitchenManager.Instance.spinnerHighlight.gameObject.SetActive(false);
		RestaurantManager.Instance.GetTable(tableNum).ToggleTableNum(false);
		var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[1]);
		Behav order = (Behav)Activator.CreateInstance(type);
		order.self = this;
		currBehav = order;
		order.Act();
		order = null;
		Waiter.Instance.Finished();
	}

	public float RandomFactor() {
		float rand = 0;
		while(rand == 0) {
			rand = UnityEngine.Random.Range(0.6f, 1.4f);
		}
        return rand;
	}

	IEnumerator SneakOut() {
		yield return new WaitForSeconds(5.0f);
		sneakOut = true;
		currBehav.Reason();
	}

	public void Annoyed() {
		isAnnoyed = true;
		var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[6]);
		Behav leave = (Behav)Activator.CreateInstance(type);
		leave.self = this;
		currBehav = leave;
		leave.Act();
		leave = null;
	}

	public void NotifyLeave() {
		var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[6]);
		Behav leave = (Behav)Activator.CreateInstance(type);
		leave.self = this;
		currBehav = leave;
		leave.Act();
		leave = null;
	}
}
