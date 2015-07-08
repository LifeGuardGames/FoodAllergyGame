using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodStockButton : MonoBehaviour {

	public string foodID;
	public Image image;
	public Text label;
	public Button button;
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = GetComponent<Localize>().GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
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
