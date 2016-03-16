using UnityEngine;
using System.Collections;

public class CustomerGossiper : Customer{

	public Behav pastBehav;

	public void GoAway(){
		Debug.Log("Behav!!!!"+ currBehav.ToString());
		customerAnim.skeletonAnim.state.SetAnimation(0, "WaitingPassive", false);
		transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).Seat);
		transform.localPosition = Vector3.zero;
		currBehav = pastBehav; 
	}
}
