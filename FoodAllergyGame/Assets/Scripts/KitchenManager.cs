using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class KitchenManager : Singleton<KitchenManager>, IWaiterSelection{
	public List<Transform> orderSpotList;
	public float cookTimer;				// times it takes to cook food

	private GameObject waiterNode;
	public GameObject WaiterNode {
		get { return waiterNode; }
	}
	
	public ChefAnimController chefAnimController;
	private int ordersCooking = 0;      // Keep an aux count for animation

	public SpriteRenderer kitchenSprite;
	public SpriteRenderer spinnerHighlight;
	public MeshRenderer chefMeshRenderer;

	private int baseSortingOrder;
	public int BaseSortingOrder {
		get {
			return baseSortingOrder;
		}
	}
	public bool IsTrachcanTut = false;
	public Animator animator;       // Used for clicking

	void Start(){
		if(SceneManager.GetActiveScene().name == SceneUtils.RESTAURANT){
			// Connect scene variables
			waiterNode = Pathfinding.Instance.NodeKitchen;
			if(DataManager.Instance.GetChallenge() != "") {
				Init(DataLoaderChallenge.GetData(DataManager.Instance.GetChallenge()).KitchenTimerMod);
			}
		}
		else{
			chefMeshRenderer.gameObject.SetActive(false);
		}
		SetBaseSortingOrder(KitchenLoader.baseSortingOrder);
        spinnerHighlight.gameObject.SetActive(false);
	}

	// changes the cooking time based off the event
	public void Init(float mode){
		cookTimer = mode;
	}

	// takes the orders from the waiter and cooks them
	public void CookOrder(List <GameObject> order){
		spinnerHighlight.gameObject.SetActive(false);
		if(order.Count > 1){
			order[0].transform.SetParent(this.gameObject.transform);
			order[0].GetComponent<Order>().StartCooking(cookTimer);
			order[1].transform.SetParent(this.gameObject.transform);
			order[1].GetComponent<Order>().StartCooking(cookTimer);
			AnimSetCooking(2);

			AudioManager.Instance.PlayClip("GiveOrder");
			AudioManager.Instance.PlayClip("Cooking", variations:3);
		}
		else if(order.Count == 1){
			order[0].transform.SetParent(this.gameObject.transform);
			order[0].GetComponent<Order>().StartCooking(cookTimer);
			AnimSetCooking(1);

			AudioManager.Instance.PlayClip("GiveOrder");
			AudioManager.Instance.PlayClip("Cooking", variations: 3);
		}
	}

	// when the order is cooked it is placed on the counter 
	public void FinishCooking(GameObject order){
		AnimSetCooking(-1);
		for(int i = 0; i < orderSpotList.Count; i ++){
			if(orderSpotList[i].transform.childCount == 0){
				order.transform.SetParent(orderSpotList[i].transform);
				order.GetComponent<Order>().SetBaseSortingOrder(baseSortingOrder + i);		// Increment by their position
				order.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
	}

	// called if a customer leaves. The order stops cooking and is destroyed
	public void CancelOrder(int tableNum){
		for(int i = 0; i < orderSpotList.Count; i++){
			if(orderSpotList[i].childCount > 0){
				if(orderSpotList[i].GetComponentInChildren<Order>().tableNumber == tableNum){
					orderSpotList[i].GetChild(0).GetComponent<Order>().Canceled();
					AnimSetCooking(-1, isFinishedCooking:false);
				}
			}
		}
	}

	public void StopCooking() {
		AnimSetCooking(-1, isFinishedCooking: false);
	}

	// Used for animator
	private void AnimSetCooking(int cookingCountDelta, bool isFinishedCooking = true){
		ordersCooking += cookingCountDelta;
		if(ordersCooking > 0) {
			chefAnimController.SetStartCooking();
		}
		else {
			ordersCooking = 0;
			if(isFinishedCooking) {
				chefAnimController.SetFinishCooking();
			}
			else {
				chefAnimController.SetCancelCooking();
			}
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(!IsTrachcanTut) {
			CookOrder(Waiter.Instance.OrderChef());
		}
		Waiter.Instance.Finished();
	}

	public void OnClicked(){
		Waiter.Instance.FindRoute(waiterNode, this);
	}

	public bool IsQueueable(){
		return true;
	}

	public virtual void OnPressAnim() {
		animator.SetTrigger("ClickPulse");
	}

	public void NotifySpinnerHighlight(){
		spinnerHighlight.gameObject.SetActive(true);
	}
	#endregion

	public void SetBaseSortingOrder(int _baseSortingOrder) {
		baseSortingOrder = _baseSortingOrder;
		kitchenSprite.sortingOrder = _baseSortingOrder;
        spinnerHighlight.sortingOrder = _baseSortingOrder + 7;
		chefMeshRenderer.sortingOrder = _baseSortingOrder + 1;
    }
}
