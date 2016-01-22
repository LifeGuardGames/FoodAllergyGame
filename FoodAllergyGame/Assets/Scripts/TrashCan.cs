using UnityEngine;
using System.Collections;

public class TrashCan : MonoBehaviour, IWaiterSelection {

	public GameObject waiterNode;
	//public GameObject spinnerHighlight;

	#region IWaiterSelection implementation
	public void OnWaiterArrived() {
		Waiter.Instance.TrashOrder();
		Waiter.Instance.Finished();
	}

	public void OnClicked() {
		Waiter.Instance.FindRoute(waiterNode, this);
	}

	public bool IsQueueable() {
		return true;
	}
	
	public void NotifySpinnerHighlight() {
		//spinnerHighlight.SetActive(true);
	}
	#endregion
}
