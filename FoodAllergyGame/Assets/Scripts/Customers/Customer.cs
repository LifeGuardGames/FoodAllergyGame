using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class Customer : MonoBehaviour, IWaiterSelection{

	public int tableNum;
	public float timer = 1.0f;
	public CustomerTypes type = CustomerTypes.Normal;
	public string customerID;			// The customer's id used for identification in the 
	public CustomerStates state;		// The current state of the customer
	public Allergies allergy;			// The allergy of the customer
	protected float menuTimer = 4.0f;	// Time spent looking at the menu
	private float attentionSpan = 15.0f;// The attention timer

	public int satisfaction;			// The satisfaction the customer has, everytime the attention span 
										//	ticks down to 0 the customer will lose satisfaction

	public FoodKeywords desiredFood;	// The keyword to help foodmanager find what the customer wants

	public GameObject order;			//the order created by the customer used to have easy access to the
										//	order should the customer leave before they eat it

	public CustomerUIController customerUI;		// ui controller for the customers handles hearts, timer icon and death icon
	public CustomerAnimController customerAnim;	// handles animations
	public GameObject OrderPrefab;				// temp variable used for instatiation
	public List <ImmutableDataFood> choices;	// a list containing possible options the user would like to eat
	public bool hasPowerUp;
	private int priceMultiplier;
	private int playAreaIndexAux;				// For use in coroutine, needs parameter with string call
    protected float spawnTime;

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
		attentionSpan = 15f * timer;
		// customers refuse to line up out the door
		if(RestaurantManager.Instance.GetLine().NewCustomer() == null){
			Destroy(this.gameObject);
		}
		else{
			// Check for fly thru table
			TableFlyThru flyThruTable = RestaurantManager.Instance.GetFlyThruTable();
			if((flyThruTable != null) && Random.Range(0,10) > 3 && !flyThruTable.inUse && Constants.GetConstant<bool>("FlyThruOn")|| mode.ID == "EventTFlyThru"){
				flyThruTable.inUse = true;
				this.gameObject.transform.SetParent(flyThruTable.seat);
				tableNum = flyThruTable.TableNumber;
				state = CustomerStates.ReadingMenu;
				StartCoroutine("ReadMenu");
				StopCoroutine("SatisfactionTimer");
				customerAnim.SetReadingMenu();
				GetComponentInParent<Table>().currentCustomerID = customerID;
				this.GetComponent<BoxCollider>().enabled = false;
				flyThruTable.FlyThruDropDown();
			}
			else{
				this.gameObject.transform.SetParent(RestaurantManager.Instance.GetLine().NewCustomer());
				RestaurantManager.Instance.lineCount++;
			}
			this.gameObject.transform.position = transform.parent.position;
		}

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
		}
	}

	// chooses an allergy cases where specific allergy is noted we use a weighted random to get the desired result
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

	// Note: Not capped
	//TODO Change character animation here???
	public void SetSatisfaction(int _satisfaction){
		satisfaction = _satisfaction;
		customerUI.UpdateSatisfaction(satisfaction);

		// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
		if(satisfaction <= 0){
			if(order != null){
				Destroy(order.gameObject);
			}
			NotifyLeave();
		}
	}

	// Note: Not capped
	public void UpdateSatisfaction(int delta){
		if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.VIP) {
			if(delta < 0){
				satisfaction += delta;
				customerUI.UpdateSatisfaction(satisfaction);
				customerAnim.UpdateSatisfaction(delta);
					
				// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
				if(satisfaction <= 0){
					if(order != null){
						Destroy(order.gameObject);
					}
					NotifyLeave();
				}
			}
		}
		else{
			satisfaction += delta;
			customerUI.UpdateSatisfaction(satisfaction);
			customerAnim.UpdateSatisfaction(delta);
			
			// If satisfaction is 0 or negative, we need to leave cause the service is rubbish
			if(satisfaction <= 0){
				if(order != null){
					Destroy(order.gameObject);
				}
				NotifyLeave();
			}
		}
	}

	// When completed removes one satisfaction from that customer
	IEnumerator SatisfactionTimer(){
		yield return new WaitForSeconds(attentionSpan);
		UpdateSatisfaction(-1);
		
		// if we still have satisfaction left we start the timer again
		StartCoroutine("SatisfactionTimer");
	}

	// Jumps to the table given a table number
	public virtual void JumpToTable(int _tableNum){
        
		RestaurantManager.Instance.lineCount--;
		RestaurantManager.Instance.CustomerLineSelectHighlightOff();
		Waiter.Instance.CurrentLineCustomer = null;
		AudioManager.Instance.PlayClip("CustomerSeated");

		//sitting down
		tableNum = _tableNum;
		transform.SetParent(RestaurantManager.Instance.GetTable(_tableNum).Seat);
		transform.localPosition = Vector3.zero;
		if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.VIP) {    // TODO connect this with logic rather than number
			RestaurantManager.Instance.VIPUses++;
			customerUI = gameObject.transform.GetComponentInParent<CustomerUIController>();
			//customerUI.satisfaction1.gameObject.SetActive(true);
			//customerUI.satisfaction2.gameObject.SetActive(true);
			//customerUI.satisfaction3.gameObject.SetActive(true);
			timer /= RestaurantManager.Instance.GetTable(_tableNum).VIPMultiplier;
			SetSatisfaction(4);
			AudioManager.Instance.PlayClip("VIPEnter");
		}
		// begin reading menu
		state = CustomerStates.ReadingMenu;
		customerAnim.SetReadingMenu();

		StartCoroutine("ReadMenu");
		// TODO-SOUND Reading menu here
		StopCoroutine("SatisfactionTimer");

		// Table connection setup
		GetComponentInParent<Table>().currentCustomerID = customerID;
		this.GetComponent<BoxCollider>().enabled = false;
        RestaurantManager.Instance.lineController.FillInLine();
    }

	// Time spent reading menu before ordering
	IEnumerator ReadMenu(){
		yield return new WaitForSeconds(menuTimer);

		//get food choices 
		choices = FoodManager.Instance.GetTwoMenuFoodChoices(desiredFood, allergy);
		customerUI.ToggleWait(true);
		//stop the satisfaction timer, change the timer and then restart it
		attentionSpan = 21.0f * timer;
		StartCoroutine("SatisfactionTimer");

		// now waiting for our order to be taken
		state = CustomerStates.WaitForOrder;
		customerAnim.SetWaitingForOrder();
		if(RestaurantManager.Instance.isTutorial){
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
				RestaurantManager.Instance.GetMenuUIController().ShowChoices(choices, tableNum, allergy);
			}
		}
		else{
			Debug.LogError("Wrong state for customer to get order: " + state.ToString());
			Waiter.Instance.Finished();
		}
	}

	// It's called when the button for food is hit get the customer to make his order and hand it to the waiter
	public virtual void OrderTaken(ImmutableDataFood food){
		if(order == null){
			state = CustomerStates.WaitForFood;
			customerAnim.SetWaitingForFood();

			customerUI.ToggleWait(false);

			order = GameObjectUtils.AddChildWithPositionAndScale(null, OrderPrefab);
			order.GetComponent<Order>().Init(food.ID, tableNum, food.AllergyList);
			priceMultiplier = food.Reward;
			RestaurantManager.Instance.GetTable(tableNum).GetComponent<Table>().OrderObtained(order);
			attentionSpan = 20.0f * timer;

			UpdateSatisfaction(1);

			StartCoroutine("SatisfactionTimer");


			// Unpause queue and continue waiter movement
			TouchManager.Instance.UnpauseQueue();
			Waiter.Instance.Finished();
		}
		else{
			Debug.LogError("Order already exists: " + order.name);
		}
	}

	// Tells the waiter the food has been delivered and begins eating
	public virtual void Eating(){
		if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.FlyThru){
			Waiter.Instance.Finished();
			if(DataManager.Instance.GetEvent() == "EventTFlyThru") {
				if(DataManager.Instance.GameData.Decoration.DecoTutQueue.Count > 0) { 
					DataManager.Instance.GameData.Decoration.DecoTutQueue.RemoveAt(0);
				}
				DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(DecoTypes.FlyThru);
				DataManager.Instance.GameData.Decoration.ActiveDeco.Add(DecoTypes.FlyThru, "FlyThru00");
				//DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
			}
            if (order.GetComponent<Order>().allergy.Contains(allergy) && allergy != Allergies.None) {
                Medic.Instance.BillRestaurant(-100);
                ParticleUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(tableNum).gameObject.transform.position, -100);


                AudioManager.Instance.PlayClip("CustomerDead");
                if (order.gameObject != null) {
                    Destroy(order.gameObject);
                }
                SetSatisfaction(0);
            }
			if(satisfaction > 0) {
				state = CustomerStates.Eating;
				NotifyLeave();
			}
		}
		else{
			UpdateSatisfaction(1);
			customerAnim.SetEating();

			order = transform.GetComponentInParent<Table>().FoodDelivered();
			order.GetComponent<BoxCollider>().enabled = false;
			order.GetComponent<Order>().ToggleShowOrderNumber(false);
			StopCoroutine("SatisfactionTimer");
			if(order.GetComponent<Order>().allergy.Contains(allergy) && allergy != Allergies.None){
				state = CustomerStates.AllergyAttack;
				AllergyAttack();
			}
			else{
				state = CustomerStates.Eating;
				StartCoroutine("EatingTimer");
				AudioManager.Instance.PlayClip("CustomerEating");
				Waiter.Instance.Finished();
			}
		}
	}

	// Eating coroutine
	IEnumerator EatingTimer(){
		yield return new WaitForSeconds(6.0f);
		customerAnim.SetWaitingForCheck();
//		int rand = Random.Range(0,10);
//		if(rand > 7){	// TODO taking out bathroom completely here
//			UseBathroom();
//			Debug.Log ("Table " + tableNum.ToString() +" has gone to the bathroom");
//		}
//		else{
			if(order.gameObject != null){
				Destroy(order.gameObject);
			}
			customerUI.ToggleStar(true);
			attentionSpan = 10.0f * timer;
			state = CustomerStates.WaitForCheck;
			StartCoroutine("SatisfactionTimer");
			AudioManager.Instance.PlayClip("CustomerReadyForCheck");
			if(RestaurantManager.Instance.isTutorial){
				this.GetComponent<CustomerTutorial>().NextTableFinger();
			}
//		}
	}

	// Tells the resturantManager that the customer is leaving and can be removed from the dictionary
	public virtual void NotifyLeave(){
		if(DataManager.Instance.GetEvent() == "EventTVIP") {
			DataManager.Instance.GameData.Decoration.DecoTutQueue.RemoveAt(0);
			DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(DecoTypes.VIP);
			//DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}

		if(DataManager.Instance.GetEvent() == "EventTPlayArea") {
			if(DataManager.Instance.GameData.Decoration.DecoTutQueue.Count > 0) {
				DataManager.Instance.GameData.Decoration.DecoTutQueue.RemoveAt(0);
			}
			DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(DecoTypes.PlayArea);
			//DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}

		if(hasPowerUp){
			//Waiter.Instance.GivePowerUp();
		}
		
		if(satisfaction > 0){
			if(satisfaction > 3) {
				satisfaction = 3;
			}
			if(state == CustomerStates.Saved || state == CustomerStates.Eaten) {
				RestaurantManager.Instance.GetTable(tableNum).inUse = false;
				RestaurantManager.Instance.CustomerLeft(this, false, satisfaction, 1, transform.position, 360f, false);
			}
			else if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.VIP) {
				RestaurantManager.Instance.CustomerLeft(this, true, satisfaction, priceMultiplier * RestaurantManager.Instance.GetTable(tableNum).VIPMultiplier, transform.position,Time.time - spawnTime, true);
			}
			else {
				RestaurantManager.Instance.CustomerLeft(this, true, satisfaction, priceMultiplier, transform.position, Time.time - spawnTime, true);
			}
		}
		else{
			RestaurantManager.Instance.CustomerLeft(this, false, satisfaction, 1, transform.position, 360f, false);
		}

		if(state != CustomerStates.InLine && state != CustomerStates.Saved){
			RestaurantManager.Instance.GetTable(tableNum).CustomerLeaving();
			if(RestaurantManager.Instance.GetTable(tableNum).tableType == Table.TableType.FlyThru) {
				RestaurantManager.Instance.GetFlyThruTable().FlyThruLeave();
			}
		}
		else if(state == CustomerStates.InLine){
			// Turn off customer highlights throughout the restaurant if it left and is selected
			if(Waiter.Instance.CurrentLineCustomer == this.gameObject){
				RestaurantManager.Instance.CustomerLineSelectHighlightOff();
			}
			transform.SetParent(null);
			RestaurantManager.Instance.lineCount--;
			RestaurantManager.Instance.lineController.FillInLine();
		}
		AudioManager.Instance.PlayClip("CustomerLeave");
        Destroy(this.gameObject);
	}

	// called if the food's ingrediants match the allergy starts a timer in which the player must hit the save me button
	public virtual void AllergyAttack(){
		priceMultiplier = 1;
		customerUI.ToggleAllergyAttack(true);
		customerAnim.SetRandomAllergyAttack();
		RestaurantManager.Instance.sickCustomers.Add(this.gameObject);
		AudioManager.Instance.PlayClip("CustomerAllergyAttackAudio");

		// Show tutorial if needed
		if(DataManager.Instance.GameData.Tutorial.IsMedicTut2Done){
			Waiter.Instance.Finished();
			StartCoroutine("AllergyTimer");
		}
		else{
			if(!DataManager.Instance.GameData.Tutorial.IsMedicTut1Done){
				DataManager.Instance.GameData.Tutorial.IsMedicTut1Done = true;
			}
			else{
				DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = true;
			}
			Waiter.Instance.isMedicTut = true;
			Waiter.Instance.CancelMove();
			RestaurantManager.Instance.medicTutorial.SetActive(true);
			//TouchManager.Instance.PauseQueue();
			string foodSpriteName = DataLoaderFood.GetData(order.GetComponent<Order>().foodID).SpriteName;
			RestaurantManager.Instance.medicTutorial.GetComponent<SickTutorialController>().Show(allergy, foodSpriteName);
		}

		// Destroy the order
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
	}

	// if they are saved they take a small penalty for making the mistake and the customer will want the check asap
	public virtual void Saved(){
		AudioManager.Instance.PlayClip("CustomerSaved");
		RestaurantManager.Instance.savedCustomers++;
		customerAnim.SetSavedAllergyAttack();
		Medic.Instance.BillRestaurant(-40);
		ParticleUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(tableNum).gameObject.transform.position,-40);
		RestaurantManager.Instance.sickCustomers.Remove(this.gameObject);
		UpdateSatisfaction(1);
		customerUI.ToggleAllergyAttack(false);
		Invoke("NotifyLeave", 4.0f);
		state = CustomerStates.Saved;
		StopCoroutine("AllergyTimer");
	}

	// when it runs out the customer is taken to the hospital and the player is slamed with the bill
	IEnumerator AllergyTimer(){
		yield return new WaitForSeconds(10.0f);
//		RestaurantManager.Instance.medicButton.GetComponent<Animator>().SetBool("TutMedic", false);
//		RestaurantManager.Instance.tutText.SetActive(false);
		RestaurantManager.Instance.sickCustomers.Remove(this.gameObject);
		Medic.Instance.BillRestaurant(-100);
		ParticleUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(tableNum).gameObject.transform.position,-100);
		DataManager.Instance.GameData.Tutorial.MissedMedic++;
		if(DataManager.Instance.GameData.Tutorial.MissedMedic >= 3) {
			DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = false;
			DataManager.Instance.GameData.Tutorial.MissedMedic = 0;
        }

		AudioManager.Instance.PlayClip("CustomerDead");
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		SetSatisfaction(0);

	}

	// Checks the current state and runs the appropriate function called by table when waiter approaches
	public virtual void CheckState(){
		switch(state){
		case CustomerStates.WaitForOrder:
			if(Waiter.Instance.Hand1 != WaiterHands.None && Waiter.Instance.Hand2 != WaiterHands.None){
				ParticleUtils.PlayHandsFullFloaty(Waiter.Instance.transform.position);
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
			NotifyLeave();
			Waiter.Instance.Finished();
			break;
		default:	// All other states, nothing to do
			Waiter.Instance.Finished();
			break;
		}
	}

	public void UseBathroom(){
		if(Constants.GetConstant<bool>("BathroomOn")){
			customerUI.satisfaction1.gameObject.SetActive(false);
			customerUI.satisfaction2.gameObject.SetActive(false);
			customerUI.satisfaction3.gameObject.SetActive(false);
			customerUI.ToggleStar(false);
			customerAnim.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
			attentionSpan = (5.0f * timer * BathRoom.Instance.timerMultipier);
			StartCoroutine("InBathroom");
		}
		else{
			if(order.gameObject != null){
				Destroy(order.gameObject);
			}
			customerUI.ToggleStar(true);
			attentionSpan = 10.0f * timer;
			state = CustomerStates.WaitForCheck;
			StartCoroutine("SatisfactionTimer");
			AudioManager.Instance.PlayClip("CustomerReadyForCheck");
		}
	}

	private IEnumerator InBathroom(){
		yield return new WaitForSeconds(attentionSpan);
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		customerUI.satisfaction1.gameObject.SetActive(true);
		customerUI.satisfaction2.gameObject.SetActive(true);
		customerUI.satisfaction3.gameObject.SetActive(true);
		customerUI.ToggleStar(true);
		satisfaction += BathRoom.Instance.deltaSatisfaction;
		customerAnim.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		customerUI.ToggleStar(true);
		attentionSpan = 10.0f * timer;
		state = CustomerStates.WaitForCheck;
		StartCoroutine("SatisfactionTimer");
		AudioManager.Instance.PlayClip("CustomerReadyForCheck");
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

		// End play
		PlayArea.Instance.EndPlayTime(playAreaIndexAux);
		transform.localPosition = Vector3.zero;	// Move the customer back to its position in line
		GetComponent<BoxCollider>().enabled = true;
		StartCoroutine("SatisfactionTimer");
	}
}
