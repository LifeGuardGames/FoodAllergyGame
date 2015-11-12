using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenManager : Singleton<KitchenManager>, IWaiterSelection{
	public List<Transform> orderSpotList;
	public float cookTimer;				// times it takes to cook food
	public GameObject waiterNode;

	public Animator kitchenAnimator;
	private int ordersCooking = 0;		// Keep an aux count for animation

	public GameObject spinnerHighlight;
	public GameObject chefParent;

	void Start(){
		if(Application.loadedLevelName == SceneUtils.RESTAURANT){
			// Connect scene variables
			waiterNode = Pathfinding.Instance.NodeKitchen;
		}
		else{
			chefParent.SetActive(false);
		}
		spinnerHighlight.SetActive(false);
	}

	// changes the cooking time based off the event
	public void Init(float mode){
		cookTimer = mode;
	}

	// takes the orders from the waiter and cooks them
	public void CookOrder(List <GameObject> order){
		spinnerHighlight.SetActive(false);
		if(order.Count > 1){
			order[0].transform.SetParent(this.gameObject.transform);
			order[0].GetComponent<Order>().StartCooking(cookTimer);
			order[1].transform.SetParent(this.gameObject.transform);
			order[1].GetComponent<Order>().StartCooking(cookTimer);
			AnimSetCooking(1);

			AudioManager.Instance.PlayClip("GiveOrder");
		}
		else if(order.Count == 1){
			order[0].transform.SetParent(this.gameObject.transform);
			order[0].GetComponent<Order>().StartCooking(cookTimer);
			AnimSetCooking(1);

			AudioManager.Instance.PlayClip("GiveOrder");
		}
	}

	// when the order is cooked it is placed on the counter 
	public void FinishCooking(GameObject order){
		AnimSetCooking(-1);
		for(int i = 0; i < orderSpotList.Count; i ++){
			if(orderSpotList[i].transform.childCount == 0){
				order.transform.SetParent(orderSpotList[i].transform);
				order.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
	}

	// called if a customer leaves. The order stops cooking and is destroyed
	public void CancelOrder(int tableNum){
		for(int i = 0; i < orderSpotList.Count; i++){
			if(orderSpotList[i].childCount > 0){
				if(orderSpotList[i].GetComponentInChildren<Order>().tableNumber == tableNum){
					orderSpotList[i].GetChild(0).GetComponent<Order>().Canceled();
					AnimSetCooking(-1);
				}
			}
		}
	}

	// Used for animator
	private void AnimSetCooking(int cookingCountDelta){
		ordersCooking += cookingCountDelta;
		if(ordersCooking <= 0){
			ordersCooking = 0;
		}
		kitchenAnimator.SetInteger("CookingCount", ordersCooking);
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		CookOrder(Waiter.Instance.OrderChef());
		Waiter.Instance.Finished();
	}

	public void OnClicked(){
		Waiter.Instance.FindRoute(waiterNode, this);
	}

	public bool IsQueueable(){
		return true;
	}

	public void NotifySpinnerHighlight(){
		spinnerHighlight.SetActive(true);
	}
	#endregion
}
