using UnityEngine;
using System.Collections;

public class CustomerGossiper : Customer{

	public Behav pastBehav;

	public void GoAway(){
		Debug.Log("Behav!!!!"+ currBehav.ToString());
		Debug.Log("PastBehav!!!!" + pastBehav.ToString());
		customerAnim.skeletonAnim.state.SetAnimation(0, "WaitingPassive", false);
		transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).Seat);
		transform.localPosition = Vector3.zero;
		if(currBehav.ToString() == "BehavGossipEating") {
			StartCoroutine("EatingTimer");
		}
		currBehav = pastBehav; 
		if(currBehav.ToString() == "BehavGossipReadMenu") {
			currBehav.Reason();
		}
		
	}
}
