using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodInfoButton : MonoBehaviour {
	public string foodID;
	public Image image;
	public Text label;
	public Button button;
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = LocalizationText.GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
	}
	
	public void OnButtonClick(){
		if(string.Equals(Application.loadedLevelName, SceneUtils.START)){
			InfoManager.Instance.ShowDetail(InfoType.Customer, foodID);
		}
	}
}
