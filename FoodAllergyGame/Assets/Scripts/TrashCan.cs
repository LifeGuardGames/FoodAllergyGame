using UnityEngine;
using System.Collections;

public class TrashCan : MonoBehaviour, IWaiterSelection {
	public GameObject trashCanNode;
	public Animation putTrashAnimation;

	#region IWaiterSelection implementation
	public void OnWaiterArrived() {
		Waiter.Instance.TrashOrder();
		Waiter.Instance.Finished();
		putTrashAnimation.Play();
		AudioManager.Instance.PlayClip("TrashCanPut");
    }

	public void OnClicked() {
		Waiter.Instance.FindRoute(trashCanNode, this);
	}

	public bool IsQueueable() {
		return true;
	}
	#endregion
}
