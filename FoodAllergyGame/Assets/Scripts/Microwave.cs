using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Microwave :Singleton<Microwave>, IWaiterSelection{

	public float cookTimer;
	public Transform waiterSpot;

	public void CookOrder(GameObject order){
		if( order != null){
			order.transform.SetParent(this.gameObject.transform);
			order.GetComponent<Order>().StartCooking(cookTimer);
			//order[0].SetActive(false);
			//StartCoroutine(Cooking(order[0]));
			//AnimSetCooking(1);
			//order[1].SetActive(false);
			//StartCoroutine(Cooking(order[1]));
			//AudioManager.Instance.PlayClip("giveOrder");
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		//CookOrder(Waiter.Instance.OrderChef());
		Waiter.Instance.Finished();
	}
	
	public void OnClicked(){
		//		if(!TouchManager.IsHoveringOverGUI()){
		Waiter.Instance.MoveToLocation(waiterSpot.position, this);
		//		}
	}
	#endregion

}
