using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelInfoController : MonoBehaviour {

	public GameObject foodInfoParent;		// Parent to all the food info game objects
	public Text foodTitleText;
	public Image foodImage;
	public Text foodAllergiesLabel;
	public Image allergy1Image;

	public GameObject customerInfoParent;	// Parent to all the customer info game objects
	public Text customerTitleText;
	public Image customerImage;
	public Text customerDescriptionText;

	void Start(){
		ToggleVisibility(false, InfoType.None);
		this.gameObject.AddComponent<Localize>();
	}

	public void ToggleVisibility(bool isVisible, InfoType infoType){
		if(!isVisible){
			foodInfoParent.SetActive(false);
			customerInfoParent.SetActive(false);
		}
		// Show the appropriate text and image objects
		else{
			switch(infoType){
			case InfoType.Food:
				foodInfoParent.SetActive(true);
				customerInfoParent.SetActive(false);
				break;

			case InfoType.Customer:
				foodInfoParent.SetActive(false);
				customerInfoParent.SetActive(true);
				break;

			default:
				Debug.LogError("Bad info type");
				break;
			}
		}
	}

	public void ShowInfo(InfoType infoType, string ID){
		switch(infoType){
		case InfoType.Food:
			ImmutableDataFood foodData = DataLoaderFood.GetData(ID);
			foodImage.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
			foodTitleText.text = GetComponent<Localize>().GetText(foodData.FoodNameKey);
			allergy1Image.enabled = false;

			// Concat the allergies list
			foodAllergiesLabel.text = "";
			for(int i = 0; i < foodData.AllergyList.Count; i++){
				// UNDONE Only show one allergy text and image for now, index 0
				foodAllergiesLabel.text = foodData.AllergyList[0].ToString();
				if(foodData.AllergyList[0] != Allergies.None){
					allergy1Image.enabled = true;
					allergy1Image.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
				}
			}
			ToggleVisibility(true, infoType);
			break;

		case InfoType.Customer:
			ImmutableDataCustomer customerData = DataLoaderCustomer.GetData(ID);
			customerImage.sprite = SpriteCacheManager.Instance.GetCustomerSpriteData(customerData.SpriteName);
			customerTitleText.text = GetComponent<Localize>().GetText(customerData.CustomerNameKey);
			customerDescriptionText.text = GetComponent<Localize>().GetText(customerData.CustomerDescription);
			ToggleVisibility(true, infoType);
			break;

		default:
			Debug.LogError("Bad info type");
			break;
		}
	}
}
