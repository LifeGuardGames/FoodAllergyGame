using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Handles the interaction between the customer and the waiter telling the waiter what action to perform based on the customers state
public class Table : MonoBehaviour, IWaiterSelection{

	public enum TableType{
		Normal,
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

	private GameObject node;
	public GameObject Node{
		get{ return node; }
	}
	public int VIPMultiplier;

	public Transform foodSpot;
	public bool inUse = false;
	public string currentCustomerID;
	public bool isBroken;
	public GameObject _canvas;
	public bool isGossiped;
	public GameObject tableHighlight;
	public SpriteRenderer tableSprite;

	void Start(){
		if(Application.loadedLevelName == SceneUtils.RESTAURANT){
			// Add youself to the list of tables
			RestaurantManager.Instance.TableList.Add(gameObject);

			// Get its node dynamically, which are pre-populated
			switch(tableType){
			case TableType.Normal:
				node = Pathfinding.Instance.GetNormalTableNode(tableNumber);
				break;
			case TableType.VIP:
				node = Pathfinding.Instance.NodeVIP;
				break;
			case TableType.FlyThru:
				node = Pathfinding.Instance.NodeFlyThru;
				break;
			}
		}
		TurnOffHighlight();
		// can't get reference to diabled objects during runtime
		_canvas.SetActive(true);
		_canvas.GetComponentInChildren<Text>().text = (tableNumber + 1).ToString();
		_canvas.SetActive(false);
	}

	//facilitates talk between customer and waiter
	public void TalkToConsumer(){
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
		_canvas.SetActive(false);
		return Waiter.Instance.HandMeal(tableNumber);
	}

	//Passes order from customer to waiter
	public void OrderObtained(GameObject order){
		Waiter.Instance.WriteDownOrder(order);
		_canvas.SetActive(true);
	}

	//makes sure there is no left over food should a customer leave ealy
	public void CustomerLeaving(){
		inUse = false;
		Waiter.Instance.RemoveMeal(tableNumber);
		RestaurantManager.Instance.Kitchen.CancelOrder(tableNumber);
	}

	//in the unfortunate circumstance a customer gets eaten we need to take care of the mess
	public void CustomerEaten(){
		if(foodSpot.childCount > 0){
			Destroy(foodSpot.GetChild(0));
		}
		RestaurantManager.Instance.GetMenuUIController().CancelOrder(tableNumber);
		GetComponentInChildren<Customer>().state = CustomerStates.Invalid;
		RestaurantManager.Instance.CustomerLeft(currentCustomerID, 0,1, transform.position);
		CustomerLeaving();
	}

	//for use by sir table smasher when he does his thing
	public void TableSmashed(){
		isBroken = true;
	}

	public void TurnOnHighlight(){
		if(tableHighlight != null && !inUse){
			tableHighlight.SetActive(true);
		}
	}
	
	public void TurnOffHighlight(){
		if(tableHighlight != null){
			tableHighlight.SetActive(false);
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(!isBroken && seat.childCount > 0){
			Waiter.Instance.CurrentTable = tableNumber;
			TalkToConsumer();

		}
		else{
			Waiter.Instance.Finished();
		}
	}

	public bool IsQueueable(){
		if(!inUse && Waiter.Instance.CurrentLineCustomer != null){
			return false;
		}
		else{
			return true;
		}
	}

	public void OnClicked(){

		if(tableType != TableType.FlyThru){
			if(Waiter.Instance.CurrentLineCustomer != null && !inUse && !isBroken){
				Waiter.Instance.CurrentLineCustomer.transform.localScale = Vector3.one;
				Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().JumpToTable(tableNumber);
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
}
