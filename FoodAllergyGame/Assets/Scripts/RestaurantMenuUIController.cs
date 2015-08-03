using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class RestaurantMenuUIController : MonoBehaviour {

	public GameObject button1;
	public Image button1Image;
	public GameObject button2;
	public Image button2Image;

	public GameObject allergyPassParent;
	public GameObject allergyFailParent;
	public Image allergyImage;
	public Text allergyText;

	public TweenToggle menuTweenToggle;

	// Current customer menu choosing info
	private List <ImmutableDataFood> choices;
	private int tableNum;
	private Allergies allergy;

	void Start(){
		choices = new List<ImmutableDataFood>();
	}

	public void ShowChoices(List <ImmutableDataFood> customerFoodChoices, int customerTableNum, Allergies customerAllergy){
		Debug.Log("SHOWING CHOICES + " + System.DateTime.Now);

		////
		Allergies auxAllergy = Allergies.None;
		int rand1 = 0;
		int choicesCount1 = 0;
		int rand2 = 0;
		int choicesCount2 = 0;
		////

		try{
			tableNum = customerTableNum;
			allergy = customerAllergy;
			choices = new List<ImmutableDataFood>();
			if(allergy == Allergies.None){
				allergyPassParent.SetActive(true);
				allergyFailParent.SetActive(false);
			}
			else{
				allergyPassParent.SetActive(false);
				allergyFailParent.SetActive(true);
				allergyImage.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(allergy);
				allergyText.text = LocalizationText.GetText("AllergyFailPrefix") + LocalizationText.GetText(allergy.ToString());
			}
			int rand = UnityEngine.Random.Range (0, customerFoodChoices.Count);
			rand1 = rand;	////
			choicesCount1 = customerFoodChoices.Count;	////
			button1Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(customerFoodChoices[rand].SpriteName);
			choices.Add(customerFoodChoices[rand]);
			customerFoodChoices.RemoveAt(rand);
			rand = UnityEngine.Random.Range (0, customerFoodChoices.Count);
			rand2 = rand;	////
			choicesCount2 = customerFoodChoices.Count;	////
			button2Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(customerFoodChoices[rand].SpriteName);
			choices.Add(customerFoodChoices[rand]);
			menuTweenToggle.Show();
			if(RestaurantManager.Instance.isTutorial && RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
				RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().step = 2;
				RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
			}
			else if (RestaurantManager.Instance.isTutorial && !RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
				RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 5;
				RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
			}

			auxAllergy = allergy;	////
		}
		catch(Exception e){
			Debug.LogError("MANUAL EXCEPTION CAUGHT : " + e.ToString());
			Debug.LogError(auxAllergy.ToString() + " " + rand1.ToString() + " " + choicesCount1.ToString() + " | " + rand2.ToString() + " " + choicesCount2.ToString());
		}
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
