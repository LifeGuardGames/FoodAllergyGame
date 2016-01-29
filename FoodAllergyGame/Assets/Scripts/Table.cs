using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Handles the interaction between the customer and the waiter telling the waiter what action to perform based on the customers state
using UnityEngine.SceneManagement;


public class Table : MonoBehaviour, IWaiterSelection{

	public enum TableType{
		Normal,
		//VIP tables do not gain hearts but increases payout
		VIP,
		FlyThru,
	}

	public TableType tableType;

	// Table Number hard coded number to distinguish between tables, from TableLoader
	public int tableNumber;
	public int TableNumber{
		get{ return tableNumber; }
		set{ tableNumber = value; }
	}

	public Transform seat;
	public Transform Seat{
		get{ return seat; }
	}

	public int seatLayerOrder;	// Sprite order, should be one below table order
	public int SeatLayerOrder{
		get{ return seatLayerOrder; }
	}

	protected GameObject node;
	public GameObject Node{
		get{ return node; }
	}
	public int VIPMultiplier;

	public Transform foodSpot;
	public bool inUse = false;
	public string currentCustomerID;
	public bool isBroken;
	public GameObject _canvas;
	public Text text;
	public bool isGossiped;
	public GameObject tableHighlight;
	public GameObject spriteParent;

	void Start(){
		Init();
	}

	public virtual void Init() {
		if(SceneManager.GetActiveScene().name == SceneUtils.RESTAURANT) {
			// Add youself to the list of tables
			RestaurantManager.Instance.TableList.Add(gameObject);

			// Get its node dynamically, which are pre-populated
			if(tableType == TableType.Normal) {
				node = Pathfinding.Instance.GetNormalTableNode(tableNumber);
			}
			if(DataManager.Instance.GetEvent() == "EventTVIP") {
				this.GetComponent<BoxCollider>().enabled = false;
			}
		}
		TurnOffHighlight();
		// can't get reference to disabled objects during runtime
		ToggleTableNum(true);
		text.text = (tableNumber + 1).ToString();
		ToggleTableNum(false);
	}

	//facilitates talk between customer and waiter
	public virtual void TalkToConsumer(){
		
		if(inUse){
			// CheckState will handle waiter finish
			transform.GetComponentInChildren<Customer>().CheckState();
		}
		else{	// Nothing to do here
			Waiter.Instance.Finished();
		}
	}

	// tell waiter to put food down
	public GameObject FoodDelivered(){
		ToggleTableNum(false);
		return Waiter.Instance.HandMeal(tableNumber);
	}

	//Passes order from customer to waiter
	public void OrderObtained(GameObject order){
		Waiter.Instance.WriteDownOrder(order);
		ToggleTableNum(true);
	}

	//makes sure there is no left over food should a customer leave ealy
	public virtual void CustomerLeaving(){
		inUse = false;
		Waiter.Instance.RemoveMeal(tableNumber);
		KitchenManager.Instance.CancelOrder(tableNumber);
        RestaurantManager.Instance.GetMenuUIController().CancelOrder(tableNumber);
		ToggleTableNum(false);
	}

	//in the unfortunate circumstance a customer gets eaten we need to take care of the mess
	public virtual void CustomerEaten(){
		if(foodSpot.childCount > 0){
			Destroy(foodSpot.GetChild(0));
		}
		RestaurantManager.Instance.GetMenuUIController().CancelOrder(tableNumber);
		Customer customerToEat = GetComponentInChildren<Customer>();
		customerToEat.state = CustomerStates.Eaten;
		RestaurantManager.Instance.CustomerLeftSatisfaction(customerToEat, false);
		CustomerLeaving();
	}
	
	//for use by sir table smasher when he does his thing
	public virtual void TableSmashed(){
		isBroken = true;
		
		// Turn off all components
		if(tableHighlight != null) {
			tableHighlight.SetActive(false);
		}
		ToggleTableNum(false);
		this.GetComponent<BoxCollider>().enabled = false;

		
		RestaurantManager.Instance.GetMenuUIController().CancelOrder(tableNumber);
		ParticleUtils.PlayTableSmashedParticle(transform.position);
		Invoke("TableSmashedCleanup", 4f);
		// TODO balance this
	}

	public virtual void TableSmashedCleanup() {
		spriteParent.SetActive(false);
    }

	public virtual void TurnOnHighlight(){
		if(tableHighlight != null && !inUse){
			tableHighlight.SetActive(true);
		}
	}
	
	public virtual void TurnOffHighlight(){
		if(tableHighlight != null){
			tableHighlight.SetActive(false);
		}
	}

	

	#region IWaiterSelection implementation
	public virtual void OnWaiterArrived(){
		if(!isBroken && seat.childCount > 0){
			Waiter.Instance.CurrentTable = tableNumber;
			TalkToConsumer();

		}
		else{
			Waiter.Instance.Finished();
		}
	}

	public virtual bool IsQueueable(){
		if(!inUse && Waiter.Instance.CurrentLineCustomer != null){
			return false;
		}
		else{
			return true;
		}
	}

	public virtual void OnClicked(){
		if(tableType != TableType.FlyThru){
			if(Waiter.Instance.CurrentLineCustomer != null && !inUse && !isBroken){
				Waiter.Instance.CurrentLineCustomer.transform.localScale = Vector3.one;
				Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().tableNum = tableNumber;
				Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().currBehav.Reason();
                inUse = true;
			}
			else if(isGossiped){
			node.transform.GetChild(0).GetComponent<CustomerGossiper>().GoAway();
			isGossiped = false;
			}
			// Move the waiter to the table to do what it does
			else{
				Waiter.Instance.FindRoute(node,this);
			}
		}
		else{
			Waiter.Instance.FindRoute(node,this);
		}
	}
	#endregion

	public void ToggleTableNum(bool onOrOff) {
			_canvas.SetActive(onOrOff);
	}
}
