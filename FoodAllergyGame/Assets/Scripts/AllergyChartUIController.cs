//using UnityEngine;
//using System.Collections;
//
//public class AllergyChartUIController : MonoBehaviour {
//
//	public TweenToggle tween;
//	public GameObject grid;
//	private GameObject entry;
//	public GameObject chartEntryPrefab;
//
//
//	void Start(){
//
//	}
//
//	public void OnOpenButton(){
//		entry = GameObjectUtils.AddChildGUI(grid, chartEntryPrefab);
//		entry.GetComponent<AllergyChartUIEntry>().Init( RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<Customer>().allergy);
//		
//		if(RestaurantManager.Instance.isTutorial && RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
//			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
//			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 3;
//			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
//		}
//		tween.Show();
//	}
//
//	public void OnCloseButton(){
//		if(RestaurantManager.Instance.isTutorial){
//			GameObject MenuUi = GameObject.Find ("RestaurantMenu");
//			MenuUi.GetComponent<RestaurantMenuUIController>().button1.SetActive(true);
//			MenuUi.GetComponent<RestaurantMenuUIController>().button2.SetActive(true);
//			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
//			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 5;
//			RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
//		}
//		Destroy(entry);
//		tween.Hide();
//	}
//}
