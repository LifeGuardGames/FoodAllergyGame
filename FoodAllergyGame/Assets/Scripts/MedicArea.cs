using UnityEngine;
using System.Collections;

public class MedicArea : MonoBehaviour, IWaiterSelection{
	public Animation pulseAnim;
	private GameObject node;

	void Start(){
		node = Pathfinding.Instance.NodeMedic;
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		pulseAnim.Play();
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
	#endregion
}
