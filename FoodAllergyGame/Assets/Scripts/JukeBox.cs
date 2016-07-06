using UnityEngine;
using System.Collections;

public class JukeBox : Singleton<JukeBox>, IWaiterSelection {

	public GameObject waiterNode;

	public void OnWaiterArrived() {
		AudioManager.Instance.JukeBox();
	}

	public void OnClicked() {
		Waiter.Instance.FindRoute(waiterNode, this);
	}

	public bool IsQueueable() {
		return true;
	}

	public virtual void OnPressAnim() {
		//animator.SetTrigger("ClickPulse");
	}
}
