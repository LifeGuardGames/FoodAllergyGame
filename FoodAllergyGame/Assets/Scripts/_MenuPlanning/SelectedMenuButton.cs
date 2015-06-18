using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedMenuButton : MonoBehaviour {

	public string foodID;
	public Image image;
	public Text label;
	public Button button;

	public void Init(string _foodID){
		foodID = _foodID;
		gameObject.name = _foodID;
		ImmutableDataFood foodData = DataLoaderFood.GetData(_foodID);
		label.text = foodData.FoodNameKey;
		image.sprite = SpriteCacheManager.Instance.GetSpriteData(foodData.SpriteName);
	}

	public void OnButtonClick(){
		// Add food to the selected menu
		MenuManager.Instance.RemoveFoodFromMenuList(foodID);
		
		// Show description on the info panel
		
	}
}
