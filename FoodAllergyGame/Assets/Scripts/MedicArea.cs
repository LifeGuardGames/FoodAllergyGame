using UnityEngine;
using System.Collections;

public class MedicArea : MonoBehaviour, IWaiterSelection{

	public GameObject node;

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
			Waiter.Instance.FindRoute(node, this);
	}
	#endregion
}
