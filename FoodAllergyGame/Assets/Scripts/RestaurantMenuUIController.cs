using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
		int rand = Random.Range (0, customerFoodChoices.Count);
		button1Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(customerFoodChoices[rand].SpriteName);
		choices.Add(customerFoodChoices[rand]);
		customerFoodChoices.RemoveAt(rand);
		rand = Random.Range (0, customerFoodChoices.Count);
		button2Image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(customerFoodChoices[rand].SpriteName);
		choices.Add(customerFoodChoices[rand]);
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
