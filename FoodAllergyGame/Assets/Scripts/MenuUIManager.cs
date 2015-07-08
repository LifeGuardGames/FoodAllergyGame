using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuUIManager : MonoBehaviour {

	public GameObject button1;
	public Image button1Image;
	public GameObject button2;
	public Image button2Image;
	public List <ImmutableDataFood> choices;
	public int tableNum;
	public GameObject order;
	public TweenToggle menuTweenToggle;

	void Start(){
		choices = new List<ImmutableDataFood>();
	}

	public void ShowChoices(List <ImmutableDataFood> foodChoices, int table){
		choices = foodChoices;
		tableNum = table;
//		Debug.Log (foodChoices[0].ID.ToString());
		button1Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodChoices[0].SpriteName);
		button2Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodChoices[1].SpriteName);
		
		menuTweenToggle.Show();
	}

	public void CancelOrder(int table){
		if(table == tableNum){
			menuTweenToggle.Hide();
		}
	}

	public void ProduceOrder(int choice){
		menuTweenToggle.Hide();

		//if(Waiter.Instance.CheckHands()){
		//Waiter.Instance.WriteDownOrder(orderObj);
		//}
		RestaurantManager.Instance.GetTable(tableNum).Seat.GetChild(0).gameObject.GetComponent<Customer>().OrderTaken(choices[choice]);
	}
}
