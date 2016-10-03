using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RestaurantMenuUIController : MonoBehaviour {
	public Button button1;
	public Image button1Image;
	public List<RestMenuButtonAllergyNode> button1AllergyNodeList;
	private int button1AllergyNodeCount;
	public Image button1Coins;

	public Button button2;
	public Image button2Image;
	public List<RestMenuButtonAllergyNode> button2AllergyNodeList;
	private int button2AllergyNodeCount;
	public Image button2Coins;

	private bool button1AuxSet = false;	// Used for button locking for tween
	private bool button2AuxSet = false;

	public Animation inspectAnimation;
	public GameObject allergyButtonParent;
	public Animator allergyButtonAnimator;
	
	public Image customerAllergy1;
	public Image customerAllergy2;
	public Image customerAllergy3;
	public Text allergyText;

	public TweenToggleDemux menuTweenToggle;

	// Current customer menu choosing info
	private ImmutableDataFood[] choices;
	private int tableNum;
	private Allergies allergy;

	void Start() {
		choices = new ImmutableDataFood[2];
	}

	public void ShowChoices(List <ImmutableDataFood> customerFoodChoices, int customerTableNum, List<Allergies> customerAllergyList){
		tableNum = customerTableNum;
		allergy = customerAllergyList[0];

		string allergyString = "";

		// Set not interactable by default, but set to true on complete tween
		button1.interactable = false;
		button2.interactable = false;
		button1AuxSet = true;
		button2AuxSet = true;

		switch(customerAllergyList.Count) {
			case 1:
				if(allergy == Allergies.None) {
					allergyString = LocalizationText.GetText("AllergyPassPrefix");
					customerAllergy1.gameObject.SetActive(false);
					customerAllergy2.gameObject.SetActive(false);
					customerAllergy3.gameObject.SetActive(false);
				}
				else {
					allergyString = LocalizationText.GetText("AllergyFailPrefix") + 
						LocalizationText.GetText(customerAllergyList[0].ToString());
					customerAllergy1.gameObject.SetActive(true);
					customerAllergy1.transform.localPosition = new Vector3(0, 120f, 0);
					customerAllergy1.sprite = SpriteCacheManager.GetAllergySpriteData(customerAllergyList[0]);
					customerAllergy2.gameObject.SetActive(false);
					customerAllergy3.gameObject.SetActive(false);
				}
				break;
			case 2:
				allergyString = LocalizationText.GetText("AllergyFailPrefix") + 
					LocalizationText.GetText(customerAllergyList[0].ToString()) + " and " +
					LocalizationText.GetText(customerAllergyList[1].ToString());
				customerAllergy1.gameObject.SetActive(true);
				customerAllergy1.transform.localPosition = new Vector3(-85f, 120f, 0);
				customerAllergy1.sprite = SpriteCacheManager.GetAllergySpriteData(customerAllergyList[0]);
				customerAllergy2.gameObject.SetActive(true);
				customerAllergy2.transform.localPosition = new Vector3(85f, 120f, 0);
				customerAllergy1.sprite = SpriteCacheManager.GetAllergySpriteData(customerAllergyList[1]);
				customerAllergy3.gameObject.SetActive(false);
				break;
			case 3:
				allergyString = LocalizationText.GetText("AllergyFailPrefix") + 
					LocalizationText.GetText(customerAllergyList[0].ToString()) + ", " +
					LocalizationText.GetText(customerAllergyList[1].ToString()) + ", and " +
					LocalizationText.GetText(customerAllergyList[2].ToString());
				customerAllergy1.gameObject.SetActive(true);
				customerAllergy1.transform.localPosition = new Vector3(-135, 120f, 0);
				customerAllergy1.sprite = SpriteCacheManager.GetAllergySpriteData(customerAllergyList[0]);
				customerAllergy1.gameObject.SetActive(true);
				customerAllergy2.transform.localPosition = new Vector3(0, 120f, 0);
				customerAllergy1.sprite = SpriteCacheManager.GetAllergySpriteData(customerAllergyList[1]);
				customerAllergy1.gameObject.SetActive(true);
				customerAllergy3.transform.localPosition = new Vector3(135, 120f, 0);
				customerAllergy1.sprite = SpriteCacheManager.GetAllergySpriteData(customerAllergyList[2]);
				break;
			default:
				Debug.Log("Bad customer allergy list count " + customerAllergyList.Count);
				break;
		}
		
		allergyButtonParent.SetActive(true);
		allergyButtonAnimator.Play("Normal");
		allergyText.text = "\"" + allergyString + "\"";

		if(RestaurantManager.Instance.isTutorial && RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
			button1AuxSet = false;
			button2AuxSet = false;
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

		AudioManager.Instance.PlayClip("MenuOpen");
	}

	public void OnShowComplete(){
		button1.interactable = button1AuxSet;
		button2.interactable = button2AuxSet;
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

			button1Coins.sprite = SpriteCacheManager.GetSlotSpriteData(foodData.Reward);
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

			button2Coins.sprite = SpriteCacheManager.GetSlotSpriteData(foodData.Reward);
		}
	}

	// Toggle the inspect food button when the user is pauses for x time
	private IEnumerator StartAnimation(){
		yield return new WaitForSeconds(2f);
		inspectAnimation.Play();
	}

	public void InspectButtonClicked(){
		if(RestaurantManager.Instance.isTutorial){
			button1.interactable = true;
			button2.interactable = true;
			RestaurantManager.Instance.GetTable(Waiter.Instance.CurrentTable).Seat.GetComponentInChildren<CustomerTutorial>().hideFinger();
		}
		RestaurantManager.Instance.inspectionButtonClicked++;
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
			button1.interactable = false;
			button2.interactable = false;
		}
	}

	public void ProduceOrder(int choice){
		menuTweenToggle.Hide();
		button1.interactable = false;
		button2.interactable = false;
		foreach(RestMenuButtonAllergyNode node in button1AllergyNodeList) {
			node.Hide();
		}
		foreach(RestMenuButtonAllergyNode node in button2AllergyNodeList) {
			node.Hide();
		}
		inspectAnimation.Stop();
		RestaurantManager.Instance.GetTable(tableNum).Seat.GetChild(0).gameObject.GetComponent<Customer>().OrderTaken(choices[choice]);
	}
}
