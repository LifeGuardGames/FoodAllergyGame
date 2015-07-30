using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Order : MonoBehaviour, IWaiterSelection{
	// ID of the food
	public string foodID;
	// Table number that ordered the dish used to find which table this goes too.
	public int tableNumber;

	public float cookTimer = 5.0f;

	private bool isBurnt;
	public bool IsBurnt{
		get{return isBurnt;}
	}

	// Is this order cooked?
	private bool isCooked;
	public bool IsCooked{
		get{
			return isCooked;
		}
		set{
			isCooked = value;
			if(isCooked == true){
				OrderImage.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(DataLoaderFood.GetData(foodID).SpriteName);
				OrderImage.SetNativeSize();
			}
		}
	}

	public Allergies allergy;

	public Image OrderImage;

	// Initialize the order when it is first spawned
	public void Init(string foodID, int tableNumber, Allergies _allergy){
		this.foodID = foodID;
		this.tableNumber = tableNumber;
		isCooked = false;
		allergy = _allergy;
		this.gameObject.GetComponentInChildren<Text>().text = tableNumber.ToString();
		if(RestaurantManager.Instance.isTutorial){
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().step = 0;
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
		}
	}

	public void StartCooking(float _cookingTimer){
		if(RestaurantManager.Instance.isTutorial){
			RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
		}
		cookTimer = _cookingTimer;
		OrderImage.enabled = false;
		GetComponentInChildren<Text>().enabled = false;
		StartCoroutine("Cooking");
		cookTimer = _cookingTimer;
	}

	private IEnumerator Cooking(){
		yield return new WaitForSeconds(cookTimer);
		IsCooked = true;
		OrderImage.enabled = true;
		GetComponentInChildren<Text>().enabled = true;
		RestaurantManager.Instance.GetKitchen().FinishCooking(this.gameObject);
		AudioManager.Instance.PlayClip("orderReady");
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

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(!Waiter.Instance.HaveMeal(tableNumber)){
			Waiter.Instance.SetHand(gameObject);
			AudioManager.Instance.PlayClip("orderPickUp");
			if(RestaurantManager.Instance.isTutorial){
				RestaurantManager.Instance.GetTable(tableNumber).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
			}
		}
		Waiter.Instance.Finished();
	}

	public void OnClicked(){
		Waiter.Instance.FindRoute(KitchenManager.Instance.waiterNode, this);
	}
	#endregion
}
