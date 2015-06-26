using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Order : MonoBehaviour, IWaiterSelection{
	// ID of the food
	public string foodID;

	// Table number that ordered the dish used to find which table this goes too.
	public int tableNumber;

	public float cookTimer = 5.0f;

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
	}

	public void StartCooking(float _cookingTimer){
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
		GameObject.Find("Kitchen").GetComponent<KitchenManager>().Cooked(this.gameObject);
	}

	public void Canceled(){
		StopCoroutine("Cooking");
		Destroy(this.gameObject);
	}

	#region IWaiterSelection implementation
	public void OnWaiterArrived(){
		if(!Waiter.Instance.HaveMeal(tableNumber)){
			Waiter.Instance.SetHand(gameObject);
		}
		Waiter.Instance.Finished();
	}

	public void OnClicked(){
		Waiter.Instance.MoveToLocation(KitchenManager.Instance.waiterSpot.position, this);
	}
	#endregion
}
