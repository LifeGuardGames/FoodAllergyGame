using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedMenuButton : MonoBehaviour {

	public string foodID;
	public Text label;
	public Image image;
	public Button button;

	public void Init(ImmutableDataFood foodData){
		foodID = foodData.ID;
		label.text = foodData.FoodNameKey;
		image.sprite = SpriteCacheManager.Instance.GetSpriteData(foodData.SpriteName);

		// Assign the onclick function to add food to menu list
		button.onClick.AddListener(delegate {
			MenuManager.Instance.AddFoodToMenuList(foodID);
		});
	}
}
