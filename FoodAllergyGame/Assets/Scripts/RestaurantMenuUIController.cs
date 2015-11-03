using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class RestaurantMenuUIController : MonoBehaviour {

	public GameObject button1;
	public Image button1Image;
	public List<RestMenuButtonAllergyNode> button1AllergyNodeList;
	private int button1AllergyNodeCount;

	public GameObject button2;
	public Image button2Image;
	public List<RestMenuButtonAllergyNode> button2AllergyNodeList;
	private int button2AllergyNodeCount;

	public Animation inspectAnimation;
	public GameObject allergyButtonParent;
	public Animator allergyButtonAnimator;

	public GameObject allergyPassParent;
	public GameObject allergyFailParent;
	public Image allergyImage;
	public Text allergyText;

	public TweenToggle menuTweenToggle;

	// Current customer menu choosing info
	private ImmutableDataFood[] choices;
	private int tableNum;
	private Allergies allergy;

	void Start() {
		choices = new ImmutableDataFood[2];
	}

	public void ShowChoices(List <ImmutableDataFood> customerFoodChoices, int customerTableNum, Allergies customerAllergy){
		tableNum = customerTableNum;
		allergy = customerAllergy;

		if(allergy == Allergies.None){
			allergyPassParent.SetActive(true);
			allergyFailParent.SetActive(false);
		}
		else{
			allergyPassParent.SetActive(false);
			allergyFailParent.SetActive(true);
			allergyButtonParent.SetActive(true);
			allergyButtonAnimator.Play("Normal");
			allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
			allergyText.text = LocalizationText.GetText("AllergyFailPrefix") + LocalizationText.GetText(allergy.ToString());
		}

		if(RestaurantManager.Instance.isTutorial && RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
			button1.GetComponent<Button>().interactable = false;
			button2.GetComponent<Button>().interactable = false;
			RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().step = 2;
			RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();

			InitButton(0, customerFoodChoices[1]);
			InitButton(0, customerFoodChoices[0]);
		}
		else if (RestaurantManager.Instance.isTutorial && !RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
			RestaurantManager.Instance.GetTable(Waiter.Instance.CurrentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 5;
			RestaurantManager.Instance.GetTable(Waiter.Instance.CurrentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();

			InitButton(0, customerFoodChoices[0]);
			InitButton(1, customerFoodChoices[1]);
		}
		else{
			int randomIndex1 = UnityEngine.Random.Range(0, 2);
			int randomIndex2 = (randomIndex1 == 0) ? 1 : 0;

			InitButton(0, customerFoodChoices[randomIndex1]);
			InitButton(1, customerFoodChoices[randomIndex2]);
		}

		menuTweenToggle.Show();

		// Inspect button
		inspectAnimation.Stop();
		if(allergy != Allergies.None){
			StopCoroutine("StartAnimation");
			StartCoroutine("StartAnimation");
		}
	}

	private void InitButton(int buttonIndex, ImmutableDataFood foodData) {
		// Left button
		if(buttonIndex == 0) {
			button1Image.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);
			choices[0] = foodData;
			for(int i = 0; i < 3; i++) {
				if(i < foodData.AllergyList.Count) {
					button1AllergyNodeList[i].Init(true, foodData.AllergyList[i]);
				}
				else {
					button1AllergyNodeList[i].Init(false, Allergies.None);
				}
			}
		}
		// Right button
		else {
			button2Image.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);
			choices[1] = foodData;
			for(int i = 0; i < 3; i++) {
				if(i < foodData.AllergyList.Count) {
					button2AllergyNodeList[i].Init(true, foodData.AllergyList[i]);
				}
				else {
					button2AllergyNodeList[i].Init(false, Allergies.None);
				}
			}
		}
	}

	// Toggle the inspect food button when the user is pauses for x time
	private IEnumerator StartAnimation(){
		yield return new WaitForSeconds(2f);
		inspectAnimation.Play();
	}

	public void InspectButtonClicked(){
		if(RestaurantManager.Instance.isTutorial){
			button1.GetComponent<Button>().interactable = true;
			button2.GetComponent<Button>().interactable = true;
			RestaurantManager.Instance.GetTable(Waiter.Instance.CurrentTable).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
			//RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 5;
			//RestaurantManager.Instance.GetTable(Waiter.Instance.currentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
		}
		StopCoroutine("StartAnimation");
		inspectAnimation.Stop();
		inspectAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
		allergyButtonParent.SetActive(false);

		// Show the food allergy nodes here
		foreach(RestMenuButtonAllergyNode node in button1AllergyNodeList) {
			node.Show();
		}

		foreach(RestMenuButtonAllergyNode node in button2AllergyNodeList) {
			node.Show();
		}
	}

	public void CancelOrder(int table){
		if(table == tableNum){
			menuTweenToggle.Hide();
		}
	}

	public void ProduceOrder(int choice){
		menuTweenToggle.Hide();
		inspectAnimation.Stop();
		RestaurantManager.Instance.GetTable(tableNum).Seat.GetChild(0).gameObject.GetComponent<Customer>().OrderTaken(choices[choice]);
	}
}
