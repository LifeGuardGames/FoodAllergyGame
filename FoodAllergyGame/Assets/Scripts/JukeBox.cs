using UnityEngine;

public class JukeBox : Singleton<JukeBox>, IWaiterSelection {

	public GameObject waiterNode;
	public GameObject queueParent;

	public void OnWaiterArrived() {
		DestroyQueueUI();
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

	public void AddQueueUI() {
		GameObject check = Resources.Load("QueueUICheckMark") as GameObject;
		GameObjectUtils.AddChildGUI(queueParent, check);
	}

	public void UpdateQueueUI(int order) {
	}

	public void DestroyQueueUI() {
		Destroy(GameObjectUtils.GetLastChild(queueParent).gameObject);
	}

	public void DestroyAllQueueUI() {
		if(queueParent.transform.childCount > 0) {
			for(int i = 0; i < queueParent.transform.childCount; i++) {
				Destroy(GameObjectUtils.GetLastChild(queueParent).gameObject);
			}
		}
	}
}
