/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FoodInfoButton : MonoBehaviour {
	public string foodID;
	public Image image;
	public Text label;
	public Button button;
	
	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		gameObject.name = foodData.ID;
		label.text = LocalizationText.GetText(foodData.FoodNameKey);
		image.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);
	}
	
	public void OnButtonClick(){
		if(string.Equals(SceneManager.GetActiveScene().name, SceneUtils.START)){
			InfoManager.Instance.ShowDetail(InfoType.Customer, foodID);
		}
	}
}
*/