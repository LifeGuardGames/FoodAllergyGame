using UnityEngine;

public class Microwave :Singleton<Microwave>, IWaiterSelection{

	public float cookTimer;
	public Transform waiterSpot;
	public bool isCooking;
	public GameObject currentlyCooking;
	public GameObject microwaveNode;
	public GameObject queueParent;

	public void CookOrder(GameObject order){
		if( order != null){
			RestaurantManager.Instance.MicrowaveUses++;
			currentlyCooking = order;
			order.transform.SetParent(this.gameObject.transform);
			order.GetComponent<Order>().StartCooking(cookTimer);
			isCooking = true;
			//AnimSetCooking(1);
			//AudioManager.Instance.PlayClip("GiveOrder");
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		DestroyQueueUI();
		if(isCooking){
			Waiter.Instance.SetHand(currentlyCooking);
		}
		else{
			CookOrder(Waiter.Instance.QuickCook());
		}
		Waiter.Instance.Finished();
	}

	public bool IsQueueable(){
		return true;
	}

	public void OnClicked(){
		//		if(!TouchManager.IsHoveringOverGUI()){
		Waiter.Instance.FindRoute(KitchenManager.Instance.WaiterNode, this);
		//		}
	}

	public virtual void OnPressAnim() {
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
	#endregion

}
