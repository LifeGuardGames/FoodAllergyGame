using UnityEngine;

public class MedicArea : Singleton<MedicArea>, IWaiterSelection{
	public Animator animator;       // Used for clicking
	public GameObject queueParent;
	private GameObject node;

	void Start(){
		node = Pathfinding.Instance.NodeMedic;
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		TouchManager.Instance.UnpauseQueue();
		DestroyQueueUI();
        RestaurantManager.Instance.DeployMedic();
		Waiter.Instance.Finished();
	}
	
	public bool IsQueueable(){
			return false;
	}

	public void ReadyForCall() {
		Waiter.Instance.FindRoute(node, this);
    }


	public void OnClicked(){
		//TouchManager.Instance.EmptyQueue();
		//TouchManager.Instance.UnpauseQueue();
		if(!Waiter.Instance.moving) {
			ReadyForCall();
		}
		else {
			Waiter.Instance.BreakAndFindRoute();
		}
	}

	public virtual void OnPressAnim() {
		animator.SetTrigger("ClickPulse");
	}

	public void AddQueueUI() {
		GameObject check = Resources.Load("QueueUICheckMark") as GameObject;
		GameObjectUtils.AddChildGUI(queueParent, check);
    }

	public void UpdateQueueUI(int order) {
	}

	public void DestroyQueueUI() {
		if(queueParent.transform.childCount != 0) {
			Destroy(GameObjectUtils.GetLastChild(queueParent).gameObject);
		}
	}
	#endregion
}
