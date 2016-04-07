using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Order : MonoBehaviour, IWaiterSelection{

	public string foodID;
	public int tableNumber;		// Table number that ordered the dish used to find which table this goes too.
	public float cookTimer = 5.0f;

	private bool isBurnt;
	public bool IsBurnt{
		get{return isBurnt;}
	}

	private bool isCooked;		// Is this order cooked?
	public bool IsCooked{
		get{
			return isCooked;
		}
		set{
			isCooked = value;
			if(isCooked == true){
				orderImage.sprite = SpriteCacheManager.GetFoodSpriteData(DataLoaderFood.GetData(foodID).SpriteName);
			}
		}
	}

	public List<Allergies> allergy;
	public SpriteRenderer orderImage;
	public Canvas orderCanvas;
	public GameObject textParent;

	// Initialize the order when it is first spawned
	public void Init(string foodID, int tableNumber, List<Allergies> _allergy){
		this.gameObject.GetComponent<BoxCollider>().enabled = false;
		this.foodID = foodID;
		this.tableNumber = tableNumber;
		isCooked = false;
		allergy = _allergy;

		// Set table number and hide it for now
		textParent.GetComponentInChildren<Text>().text = (tableNumber + 1).ToString();
		ToggleShowOrderNumber(false);

		if(RestaurantManager.Instance.isTutorial){
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().step = 0;
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
		}
	}

	public void SetBaseSortingOrder(int baseSortingOrder) {
		orderImage.sortingOrder = baseSortingOrder + 4;
		orderCanvas.sortingOrder = baseSortingOrder + 5;
    }

	public void StartCooking(float _cookingTimer){
		if(RestaurantManager.Instance.isTutorial){
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
		}
		cookTimer = _cookingTimer;
		orderImage.gameObject.SetActive(false);
		StartCoroutine("Cooking");
		cookTimer = _cookingTimer;
	}

	private IEnumerator Cooking(){
		yield return new WaitForSeconds(cookTimer);
		IsCooked = true;
		orderImage.gameObject.SetActive(true);
		ToggleShowOrderNumber(true);
		this.gameObject.GetComponent<BoxCollider>().enabled = true;
		KitchenManager.Instance.FinishCooking(this.gameObject);
		AudioManager.Instance.PlayClip("OrderReady");
		if(RestaurantManager.Instance.isTutorial){
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().step = 1;
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
		}
	} 

	public void Canceled(){
		StopCoroutine("Cooking");
		Destroy(this.gameObject);
	}

	public void CookInMicrowave(){
		StartCoroutine("StartMicrowave");
	}

	IEnumerator StartMicrowave(){
		yield return new WaitForSeconds(2.5f);
		if(isCooked){
			isBurnt = true;
		}
		if(isCooked == false){
			isCooked = true;
			StartCoroutine("StartMicrowave");
		}
	}

	public void ToggleShowOrderNumber(bool isShown) {
		if(DataManager.Instance.GetChallenge() != "Challenge03") {
			textParent.SetActive(isShown);
		}
		else {
			textParent.SetActive(false);
		}
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(!Waiter.Instance.HaveMeal(tableNumber)&& Waiter.Instance.CheckHands()){

			// Wierd case where script is being called even though process of being destroyed
			if(this == null) { // WHAAAAT!?
				Waiter.Instance.Finished();
				return;
			}

			this.gameObject.GetComponent<BoxCollider>().enabled = false;
			Waiter.Instance.SetHand(gameObject);
			AudioManager.Instance.PlayClip("OrderPickUp");
			if(RestaurantManager.Instance.isTutorial){
				RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
				RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().NextTableFinger();
			}
		}
		Waiter.Instance.Finished();
	}

	public bool IsQueueable(){
		return true;
	}

	public void OnClicked(){
		Waiter.Instance.FindRoute(KitchenManager.Instance.WaiterNode, this);
	}
	#endregion
}
