﻿using UnityEngine;

public class MedicArea : MonoBehaviour, IWaiterSelection{
	public Animator animator;       // Used for clicking
	private GameObject node;

	void Start(){
		node = Pathfinding.Instance.NodeMedic;
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
        RestaurantManager.Instance.DeployMedic();
		Waiter.Instance.Finished();
	}
	
	public bool IsQueueable(){
		if(Waiter.Instance.CurrentLineCustomer != null){
			return false;
		}
		else{
			return true;
		}
	}
	
	public void OnClicked(){
		TouchManager.Instance.UnpauseQueue();
		Waiter.Instance.FindRoute(node, this);
	}

	public virtual void OnPressAnim() {
		animator.SetTrigger("ClickPulse");
	}
	#endregion
}
