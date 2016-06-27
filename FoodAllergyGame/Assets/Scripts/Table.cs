using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Handles the interaction between the customer and the waiter telling the waiter what action to perform based on the customers state
using UnityEngine.SceneManagement;


public class Table : MonoBehaviour, IWaiterSelection{

	public enum TableType{
		Normal,
		VIP,        //VIP tables do not gain hearts but increases payout
		FlyThru
	}

	public TableType tableType;
	public bool cantLeave;
	public int tableNumber;     // Table Number hard coded number to distinguish between tables, from TableLoader
	public int TableNumber{
		get{ return tableNumber; }
		set{ tableNumber = value; }
	}

	public Transform seat;
	public Transform Seat{
		get{ return seat; }
	}

	protected int baseSortingOrder;
	public int BaseSortingOrder {
		get { return baseSortingOrder; }
	}

	protected GameObject node;
	public GameObject Node{
		get{ return node; }
	}

	public int VIPMultiplier;

	public SpriteRenderer tableSprite;
	public Canvas uiCanvas;
	public SpriteRenderer tableHighlight;

	public Transform foodSpot;
	public bool inUse = false;
	public string currentCustomerID;
	public bool isBroken;
	public Text text;
	public bool isGossiped;
	public Animator animator;		// Used for clicking

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
			// can't get reference to disabled objects during runtime
			ToggleTableNum(true);
			text.text = (tableNumber + 1).ToString();
		}
		TurnOffHighlight();

		// Turning this off after it has been set
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

	//makes sure there is no left over food should a customer leave early
	public virtual void CustomerLeaving(){
		inUse = false;
		Waiter.Instance.RemoveMeal(tableNumber);
		KitchenManager.Instance.CancelOrder(tableNumber);
        RestaurantManager.Instance.MenuUIController.CancelOrder(tableNumber);
		ToggleTableNum(false);
	}

	//in the unfortunate circumstance a customer gets eaten we need to take care of the mess
	public virtual void CustomerEaten(GameObject eater){
		if(foodSpot.childCount > 0){
			Destroy(foodSpot.GetChild(0).gameObject);
		}
		RestaurantManager.Instance.MenuUIController.CancelOrder(tableNumber);
		Customer customerToEat = GetComponentInChildren<Customer>();
		customerToEat.state = CustomerStates.Eaten;
		RestaurantManager.Instance.CustomerLeftSatisfaction(customerToEat, false);
		Debug.Log("gettingcalled");
		StartCoroutine(Eaten(eater));
		//CustomerLeaving();
		inUse = false;
		Waiter.Instance.RemoveMeal(tableNumber);
		KitchenManager.Instance.CancelOrder(tableNumber);
		RestaurantManager.Instance.MenuUIController.CancelOrder(tableNumber);
		ToggleTableNum(false);
		
	}
	
	IEnumerator Eaten(GameObject eater) {
		yield return new WaitForSeconds(3.0f);
		Customer customerToEat = GetComponentInChildren<Customer>();
		customerToEat.customerAnim.skeletonAnim.GetComponent<MeshRenderer>().sortingOrder = eater.GetComponent<Customer>().customerAnim.skeletonAnim.GetComponent<MeshRenderer>().sortingOrder + 1;
		LeanTween.move(customerToEat.gameObject, eater.transform.position, 5.0f);
		customerToEat.DestroySelf(5.0f);
	}

	//for use by sir table smasher when he does his thing
	public virtual void TableSmashed(){
		isBroken = true;
		
		// Turn off all components
		if(tableHighlight != null) {
			tableHighlight.gameObject.SetActive(false);
		}
		ToggleTableNum(false);
		this.GetComponent<BoxCollider>().enabled = false;

		RestaurantManager.Instance.TableList.Remove(this.gameObject);
		RestaurantManager.Instance.actTables--;
		RestaurantManager.Instance.CheckTablesForGameOver();
		RestaurantManager.Instance.MenuUIController.CancelOrder(tableNumber);
		ParticleAndFloatyUtils.PlayTableSmashedParticle(transform.position);
		Invoke("TableSmashedCleanup", 4f);
		// TODO balance this
	}

	public virtual void TableSmashedCleanup() {
		tableSprite.gameObject.SetActive(false);
    }

	public virtual void TurnOnHighlight(){
		if(tableHighlight != null && !inUse){
			tableHighlight.gameObject.SetActive(true);
		}
	}
	
	public virtual void TurnOffHighlight(){
		if(tableHighlight != null){
			tableHighlight.gameObject.SetActive(false);
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
				inUse = true;
				Waiter.Instance.CurrentLineCustomer.transform.localScale = Vector3.one;
				Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().tableNum = tableNumber;
				Waiter.Instance.CurrentLineCustomer.GetComponent<Customer>().currBehav.Reason();
                
			}
			else if(isGossiped){
				isGossiped = false;
				if(node.transform.GetChild(0).GetComponent<CustomerGossiper>() != null) {
					node.transform.GetChild(0).GetComponent<CustomerGossiper>().GoAway();
				}
				else if(node.transform.GetChild(0).GetComponent<CustomerSpecialGossiper>()!= null) {
					node.transform.GetChild(0).GetComponent<CustomerSpecialGossiper>().GoAway();
                }
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

	public virtual void OnPressAnim() {
		animator.SetTrigger("ClickPulse");
	}
	#endregion

	public void ToggleTableNum(bool onOrOff) {
		if(DataManager.Instance.GetChallenge() != "Challenge03") {
			uiCanvas.gameObject.SetActive(onOrOff);
		}
		else {
			uiCanvas.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Base sorting order from the node placed itself
	/// </summary>
	public virtual void SetBaseSortingOrder(int _baseSortingOrder) {
		baseSortingOrder = _baseSortingOrder;
		tableSprite.sortingOrder = _baseSortingOrder + 2;
		uiCanvas.sortingOrder = _baseSortingOrder + 3;
		tableHighlight.sortingOrder = _baseSortingOrder + 5;
    }
}
