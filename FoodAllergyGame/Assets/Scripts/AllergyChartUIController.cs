using UnityEngine;
using System.Collections;

public class AllergyChartUIController : MonoBehaviour {

	public TweenToggle tween;
	public GameObject grid;
	public GameObject chartEntryPrefab;

	void Start(){
		foreach(ImmutableDataFood foodData in FoodManager.Instance.MenuList){
			GameObject entry = GameObjectUtils.AddChildGUI(grid, chartEntryPrefab);
			entry.GetComponent<AllergyChartUIEntry>().Init(foodData.ID);
		}
	}

	public void OnOpenButton(){
		if(RestaurantManager.Instance.isTutorial && RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 3;
			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
		}
		tween.Show();
	}

	public void OnCloseButton(){
		if(RestaurantManager.Instance.isTutorial){
			GameObject MenuUi = GameObject.Find ("RestaurantMenu");
			MenuUi.GetComponent<RestaurantMenuUIController>().button1.SetActive(true);
			MenuUi.GetComponent<RestaurantMenuUIController>().button2.SetActive(true);
			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 5;
			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
		}
		tween.Hide();
	}
}
