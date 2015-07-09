using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuUIManager : MonoBehaviour {

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
		choices = customerFoodChoices;
		tableNum = customerTableNum;
		allergy = customerAllergy;

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

		button1Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(customerFoodChoices[0].SpriteName);
		button2Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(customerFoodChoices[1].SpriteName);
		
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
