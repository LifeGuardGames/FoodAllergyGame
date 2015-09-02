using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class RestaurantMenuUIController : MonoBehaviour {

	public GameObject button1;
	public Image button1Image;
	public GameObject allergyNode1;
	public Image allergyNodeImage1;

	public GameObject button2;
	public Image button2Image;
	public GameObject allergyNode2;
	public Image allergyNodeImage2;

	public Animation inspectAnimation;

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
//		Debug.Log("SHOWING CHOICES + " + System.DateTime.Now);

		//// Used for debugging
		Allergies auxAllergy = Allergies.None;
		int rand1 = 0;
		int choicesCount1 = 0;
		int rand2 = 0;
		int choicesCount2 = 0;
		////

		// Reset the nodes
		allergyNode1.SetActive(false);
		allergyNode2.SetActive(false);

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
				allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
				allergyText.text = LocalizationText.GetText("AllergyFailPrefix") + LocalizationText.GetText(allergy.ToString());
			}

			if(RestaurantManager.Instance.isTutorial && RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
				button1.GetComponent<Button>().interactable = false;
				button2.GetComponent<Button>().interactable = false;
				RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().step = 2;
				RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
				button2Image.sprite = SpriteCacheManager.GetFoodSpriteData(customerFoodChoices[0].SpriteName);
				button1Image.sprite = SpriteCacheManager.GetFoodSpriteData(customerFoodChoices[1].SpriteName);
				choices.Add(customerFoodChoices[1]);
				choices.Add(customerFoodChoices[0]);
			}
			else if (RestaurantManager.Instance.isTutorial && !RestaurantManager.Instance.GetTable(customerTableNum).Seat.GetComponentInChildren<CustomerTutorial>().isAllergy){
				RestaurantManager.Instance.GetTable(Waiter.Instance.CurrentTable).Seat.GetComponentInChildren<CustomerTutorial>().step = 5;
				RestaurantManager.Instance.GetTable(Waiter.Instance.CurrentTable).Seat.GetComponentInChildren<CustomerTutorial>().nextHint();
				button2Image.sprite = SpriteCacheManager.GetFoodSpriteData(customerFoodChoices[0].SpriteName);
				button1Image.sprite = SpriteCacheManager.GetFoodSpriteData(customerFoodChoices[1].SpriteName);
				choices.Add(customerFoodChoices[0]);
				choices.Add(customerFoodChoices[1]);
			}
			else{
				int rand = UnityEngine.Random.Range (0, customerFoodChoices.Count);
				rand1 = rand;	////
				choicesCount1 = customerFoodChoices.Count;	////
				button1Image.sprite = SpriteCacheManager.GetFoodSpriteData(customerFoodChoices[rand].SpriteName);
				choices.Add(customerFoodChoices[rand]);
				customerFoodChoices.RemoveAt(rand);
				rand = UnityEngine.Random.Range (0, customerFoodChoices.Count);
				rand2 = rand;	////
				choicesCount2 = customerFoodChoices.Count;	////
				button2Image.sprite = SpriteCacheManager.GetFoodSpriteData(customerFoodChoices[rand].SpriteName);
				choices.Add(customerFoodChoices[rand]);
			}
			menuTweenToggle.Show();

			inspectAnimation.Stop();
			if(allergy != Allergies.None){
				StartCoroutine("StartAnimation");
			}

			auxAllergy = allergy;	////
		}
		catch(Exception e){
			Debug.LogError("MANUAL EXCEPTION CAUGHT : " + e.ToString());
			Debug.LogError(auxAllergy.ToString() + " " + rand1.ToString() + " " + choicesCount1.ToString() + " | " + rand2.ToString() + " " + choicesCount2.ToString());
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

		// Show the food allergy nodes here
		allergyNodeImage1.sprite = SpriteCacheManager.GetAllergySpriteData(choices[0].AllergyList[0]);
		allergyNode1.SetActive(true);

		allergyNodeImage2.sprite = SpriteCacheManager.GetAllergySpriteData(choices[1].AllergyList[0]);
		allergyNode2.SetActive(true);
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
