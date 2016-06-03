using UnityEngine;

public class CustomerGossiper : Customer{
	public Behav pastBehav;

	public override void Init(int num, ImmutableDataChallenge mode) {
		base.Init(num, mode);
		type = CustomerTypes.Gossiper;
	}

	public override void Init(int num, ImmutableDataEvents mode) {
		base.Init(num, mode);
		type = CustomerTypes.Gossiper;
	}

	public void GoAway(){
		Debug.Log("Behav!!!!"+ currBehav.ToString());
		Debug.Log("PastBehav!!!!" + pastBehav.ToString());
		customerAnim.skeletonAnim.state.SetAnimation(0, "WaitingPassive", false);

		Table originalTable = RestaurantManager.Instance.GetTable(tableNum);
        transform.SetParent(originalTable.Seat);
		SetBaseSortingOrder(originalTable.BaseSortingOrder);
		transform.localPosition = Vector3.zero;
		if(currBehav.ToString() == "BehavGossipEating") {
			StartCoroutine("EatingTimer");
			customerAnim.SetEating();
		}
		currBehav = pastBehav; 
		if(currBehav.ToString() == "BehavGossipReadMenu") {
			currBehav.Act();
		}
	}
}
