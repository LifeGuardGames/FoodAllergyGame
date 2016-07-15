using UnityEngine;
using System.Collections;

public class JukeBox : Singleton<JukeBox>, IWaiterSelection {

	public GameObject waiterNode;

	public void OnWaiterArrived() {
	}

	public void OnClicked() {
		AudioManager.Instance.JukeBox();
	}

	public bool IsQueueable() {
		return true;
	}

	public virtual void OnPressAnim() {
		//animator.SetTrigger("ClickPulse");
	}
}
