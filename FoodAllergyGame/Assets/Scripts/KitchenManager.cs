using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenManager : Singleton<KitchenManager>, IWaiterSelection{

	// CookOrder runs coroutines on orders to cook them once the coroutine is finished the food will be ready for pick up shown by 
	// MenuUIManager populates the menu sidebar for food selection

	public Transform waiterSpot;	// position of the waiter
	public List<Transform> orderSpotList;
	public float cookTimer;			// times it takes to cook food

	public Animator kitchenAnimator;
	private int ordersCooking = 0;		// Keep an aux count for animation


	//changes the cooking time based off the event
	public void Init(string mode){
		switch(mode){
		case "0":
			cookTimer = 5.0f;
			break;
		
		case "1":
			cookTimer = 6.5f;
			break;
		case "2":
			cookTimer = 3.0f;
			break;
		}
	}

	// takes the orders from the waiter and cooks them
	public void CookOrder(List <GameObject> order){
		if(order.Count > 1){
			order[0].transform.SetParent(this.gameObject.transform);
			order[0].GetComponent<Order>().StartCooking(cookTimer);
			//order[0].SetActive(false);
			//StartCoroutine(Cooking(order[0]));
			order[1].transform.SetParent(this.gameObject.transform);
			order[1].GetComponent<Order>().StartCooking(cookTimer);
			AnimSetCooking(1);
			//order[1].SetActive(false);
			//StartCoroutine(Cooking(order[1]));
			AudioManager.Instance.PlayClip("GiveOrder");
		}
		else if(order.Count == 1){
			order[0].transform.SetParent(this.gameObject.transform);
			//StartCoroutine(Cooking(order[0]));
			order[0].GetComponent<Order>().StartCooking(cookTimer);
			AnimSetCooking(1);
			//order[0].SetActive(false);
			AudioManager.Instance.PlayClip("GiveOrder");
		}
	}

	//when the order is cooked it is placed on the counter 
	public void FinishCooking(GameObject order){
		AnimSetCooking(-1);
		for(int i = 0; i < orderSpotList.Count; i ++){
			if(orderSpotList[i].transform.childCount == 0){
				//order.SetActive(true);
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
					//StopCoroutine(Cooking(orderSpotList[i].GetChild(0).gameObject));
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
		Waiter.Instance.MoveToLocation(waiterSpot.position, this);
	}
	#endregion
}
