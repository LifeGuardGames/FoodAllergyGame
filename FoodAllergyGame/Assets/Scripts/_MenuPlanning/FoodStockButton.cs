using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodStockButton : MonoBehaviour {

	public string foodID;
	public Image image;
	public Text label;
	public Image allergy1;
	public Image allergy2;
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = GetComponent<Localize>().GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);

		// Set allergy sprite indicators if present
		if(foodData.AllergyList.Count == 1){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = false;
		}
		else if(foodData.AllergyList.Count == 2){
			allergy1.enabled = true;
			allergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
			allergy2.enabled = true;
			allergy2.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[1]);
		}
	}

	public void OnButtonClick(){
		if(string.Equals(Application.loadedLevelName, SceneUtils.MENUPLANNING)){
			MenuManager.Instance.AddFoodToMenuList(foodID);	// Add food to the selected menu
		}
		else if(string.Equals(Application.loadedLevelName, SceneUtils.START)){
			InfoManager.Instance.ShowDetail(InfoType.Food, foodID);
		}
		AudioManager.Instance.PlayClip("menuButton");
	}
}
