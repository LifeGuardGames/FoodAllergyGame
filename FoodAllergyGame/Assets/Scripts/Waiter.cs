using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waiter : Singleton<Waiter>{
	private WaiterHands hand1;
	public WaiterHands Hand1{
		get{ return hand1; }
	}

	private WaiterHands hand2;
	public WaiterHands Hand2{
		get{ return hand2; }
	}

	public Transform hand1Parent;
	public Transform hand2Parent;
	private GameObject hand1Object;
	private GameObject hand2Object;

	private GameObject currentLineCustomer;
	public GameObject CurrentLineCustomer{
		get{ return currentLineCustomer; }
		set{ currentLineCustomer = value; }
	}

	private int currentTable;
	public int CurrentTable{
		get{ return currentTable; }
		set{ currentTable = value; }
	}

	private bool canMove = true;
	public bool CanMove{
		get{ return canMove; }
		set{ canMove = value; }
	}

	public WaiterAnimController waiterAnimController;
	public SpriteRenderer waiterSprite;

	public GameObject currentNode;
	private bool moving;
	private int pathIndex = 0;
	private List<GameObject> pathList;
	private GameObject targetNode;
	private IWaiterSelection currentCaller;

	void Start(){
		ResetHands();
		pathList = new List<GameObject>();
	}

	public void FindRoute(GameObject _targetNode, MonoBehaviour caller){
		CanMove = false;
		currentCaller = (IWaiterSelection)caller;
		pathList.Clear();
		if(currentCaller == null){
			Debug.LogError("No IWaiterSelection script exists in the caller");
		}
		if(currentNode == _targetNode){
			moving = true;
		}
		else{
			pathList = Pathfinding.Instance.findPath(currentNode, _targetNode);
			targetNode = _targetNode;
			moving = true;
		}
	}

//	public void MoveToLocation(List<GameObject> path, int index){
//		canMove = false;
////			currentCaller = (IWaiterSelection)caller;
////			if(currentCaller == null){
////				Debug.LogError("No IWaiterSelection script exists in the caller");
////				}
//		
//		//If the waiter is already at its location, just call what it needs to call
////			if(transform.position == location){
////				MoveDoneCallback();
////			}
//		// Otherwise, move to the location and wait for callback
////			else{
//		moving = true;
//		waiterAnimController.SetMoving(true);
////			Debug.Log (path[index].name);
//		LeanTween.cancel(gameObject);
//		LeanTween.move(gameObject, path[index].transform.position, movingTime).setOnComplete(MoveDoneCallback);
////			}
//	}
//
//	public void MoveDoneCallback(){
//		if(pathList.Count == 0){
//			if(currentCaller == null){
//				Debug.LogError("No IWaiterSelection script currently exists");
//			}
//			currentCaller.OnWaiterArrived();
//		}
//		else if(currentNode == pathList[index]){
//			index = 0;
//			// Note: Set animations to false before OnWaiterArrived
//			moving = false;
//			waiterAnimController.SetMoving(false);
//
//			if(currentCaller == null){
//				Debug.LogError("No IWaiterSelection script currently exists");
//			}
//			currentCaller.OnWaiterArrived();
//		}
//		else{
//			//canMove = true;
//			index++;
//			MoveToLocation(pathList, index);
//		}
//	}

	void FixedUpdate(){
		if(moving == true){
			// Already at the target node
			if(pathList.Count == 0){
				moving = false;
				currentCaller.OnWaiterArrived();
			}
			// Arrived at the next node
			else if(transform.position == pathList[pathIndex].transform.position){
				currentNode = pathList[pathIndex];

				// At the target node
				if(currentNode == targetNode){
					waiterAnimController.SetMoving(false);
					moving = false;
					pathIndex = 0;
					currentCaller.OnWaiterArrived();
				}
				// Not at the target node, start move to next one
				else{
					pathIndex++;
				}

				// Change the waiter image layer order
				waiterSprite.sortingOrder = currentNode.GetComponent<Node>().layerOrderBase;
			}
			// Keep moving
			else{
				transform.position = Vector3.MoveTowards(transform.position, pathList[pathIndex].transform.position,10);
			}
		}
	}

	// Restores hand to empty state
	public void ResetHands(){
		hand1 = WaiterHands.None;
		hand2 = WaiterHands.None;
	}

	// Populates first free hand with enum
	public void SetHand(GameObject order){
		if(hand1 == WaiterHands.None){
			if(!order.GetComponent<Order>().IsCooked){
				hand1 = WaiterHands.Order;
				hand1Object = order;
				hand1Object.transform.SetParent(hand1Parent);		// Show order in hand
				hand1Object.transform.localPosition = new Vector3(0, 0, 0);

				// Tell kitchen manager to highlight spinner
				KitchenManager.Instance.NotifySpinnerHighlight();
			}
			else{
				hand1 = WaiterHands.Meal;
				hand1Object = order;
				hand1Object.transform.SetParent(hand1Parent);		// Show order in hand
				hand1Object.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
		else if(hand2 == WaiterHands.None){
			if(!order.GetComponent<Order>().IsCooked){
				hand2 = WaiterHands.Order;
				hand2Object = order;
				hand2Object.transform.SetParent(hand2Parent);		// Show order in hand
				hand2Object.transform.localPosition = new Vector3(0, 0, 0);

				// Tell kitchen manager to highlight spinner
				KitchenManager.Instance.NotifySpinnerHighlight();
			}
			else{
				hand2 = WaiterHands.Meal;
				hand2Object = order;
				hand2Object.transform.SetParent(hand2Parent);		// Show order in hand
				hand2Object.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
		else{
			// Hands are full
		}
	}

	public List<GameObject> OrderChef(){
		List<GameObject> tempOrderArr = new List<GameObject>();
		if(hand2 == WaiterHands.Order && hand1 == WaiterHands.Order){
			tempOrderArr.Add(hand2Object);
			tempOrderArr.Add(hand1Object);
			hand2 = WaiterHands.None;
			hand1 = WaiterHands.None;
			return tempOrderArr;
		}
		else if(hand2 == WaiterHands.Order){
			tempOrderArr.Add(hand2Object);
			hand2Object = null;
			hand2 = WaiterHands.None;
			return tempOrderArr;
		}
		else if(hand1 == WaiterHands.Order){
			tempOrderArr.Add(hand1Object);
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempOrderArr;
		}
		else{
			return tempOrderArr;
			//nothing here
		}
	}

	public GameObject QuickCook(){
		GameObject tempOrder;
		tempOrder = null;
		if(hand1 == WaiterHands.Order){
			tempOrder = hand1Object;
			return tempOrder;
		}
		else if(hand2 == WaiterHands.Order){
			tempOrder = hand2Object;
			return tempOrder;
		}
		else{
			return tempOrder;
		}
	}

	public GameObject HandMeal(int tableNum){
		if(hand1 == WaiterHands.Meal && hand1Object.GetComponent<Order>().tableNumber == tableNum){
			GameObject tempFood = hand1Object;
			tempFood.transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).foodSpot);
			tempFood.transform.localPosition = new Vector3(0, 0, 0);
			hand1Object = null;
			hand1 = WaiterHands.None;
			return tempFood;
		}
		else if(hand2 == WaiterHands.Meal && hand2Object.GetComponent<Order>().tableNumber == tableNum){
			GameObject tempFood = hand2Object;
			tempFood.transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).foodSpot);
			tempFood.transform.localPosition = new Vector3(0, 0, 0);
			hand2Object = null;
			hand2 = WaiterHands.None;
			return tempFood;
		}
		else{
			Debug.LogError("No appropriate meal");
			Debug.LogError("Table number " + tableNum + " | " + hand1.ToString() + " | " + hand2.ToString());
			return null;
		}
	}

	public bool CheckHands(){
		if(hand2 == WaiterHands.None || hand1 == WaiterHands.None){
			return true;
		}
		else{
			return false;
		}
	}

	public void RemoveMeal(int table){
		if(hand1 != WaiterHands.None){
			if(hand1Object != null){
				if(hand1Object.GetComponent<Order>() != null){
					if(hand1Object.GetComponent<Order>().tableNumber == table){
						Destroy(hand1Object.gameObject);
						hand1 = WaiterHands.None;
					}
				}
				else{
					Debug.LogError("Object Name: " + hand1Object.name + " Table Number " + table);
				}
			}
			else{
				Debug.LogError("No Object in Hand");
			}
		}
		if(hand2 != WaiterHands.None){
			if(hand2Object != null){
				if(hand2Object.GetComponent<Order>() != null){
					if(hand2Object.GetComponent<Order>().tableNumber == table){
						Destroy(hand2Object.gameObject);
						hand2 = WaiterHands.None;
					}
				}
				else{
					Debug.LogError("Object Name: " + hand2Object.name + " Table Number " + table);
				}
			}
			else{
				Debug.LogError("No Object in Hand");
			}
		}
	}

	public bool HaveMeal(int table){
		if(hand1 == WaiterHands.Meal){
			if(hand1Object.GetComponent<Order>().tableNumber == table && hand1Object.GetComponent<Order>().IsCooked){
				return true;
			}
		}
		 if(hand2 == WaiterHands.Meal){
			if(hand2Object.GetComponent<Order>().tableNumber == table && hand2Object.GetComponent<Order>().IsCooked){
				return true;
			}
		}
		return false;
	}

	public void GivePowerUp(){
		
	}

	public void WriteDownOrder(GameObject order){
		AudioManager.Instance.PlayClip("writeDownOrder");
		SetHand(order);
	}

	public void Finished(){
		CanMove = true;
		if(TouchManager.Instance.inputQueue.Count > 0){
			//	if(TouchManager.Instance.inputQueue.Peek ().GetComponent<Table>().seat.GetComponentInChildren<Customer>().state != CustomerStates.WaitForOrder && Waiter.Instance.CheckHands()){


			//these statements allow a customer be seated if the next object in the queue is an empty table otherwise it will deselect the current customer
			// need to make sure the next item in the queue is a table
			if(TouchManager.Instance.inputQueue.Peek ().GetComponent<Table>() != null){
				//if it is and that table is full we deselect the customer
				if(TouchManager.Instance.inputQueue.Peek ().GetComponent<Table>().inUse == true){
					if(CurrentLineCustomer != null){
						CurrentLineCustomer.GetComponent<Customer>().Deselect();
					}
				}
			}
			else{
				//otherwise if we clicked on something else and we have a customer, we deselect them
				if(CurrentLineCustomer != null){
					CurrentLineCustomer.GetComponent<Customer>().Deselect();
				}
			}
			GameObject dequeuedObject = TouchManager.Instance.inputQueue.Dequeue();
			if(dequeuedObject != null){
				dequeuedObject.GetComponent<IWaiterSelection>().OnClicked();
			}
			//}
		}
	}

}
