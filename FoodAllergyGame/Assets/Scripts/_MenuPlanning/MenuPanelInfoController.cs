//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//
//public class MenuPanelInfoController : MonoBehaviour {
//
//	public TweenToggle menuPanelInfoTween;
//	public Text foodTitleText;
//	public Image foodImage;
//	public Text foodAllergiesLabel;
//	public Image allergy1Image;
//
//	public void Show(string foodID){
//		ShowInfo(foodID);
//		menuPanelInfoTween.Show();
//	}
//
//	public void Hide(){
//		menuPanelInfoTween.Hide();
//	}
//	
//	private void ShowInfo(string foodID){
//		ImmutableDataFood foodData = DataLoaderFood.GetData(foodID);
//		foodImage.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
//		foodTitleText.text = foodTitleText.GetComponent<Localize>().GetText(foodData.FoodNameKey);
//		allergy1Image.enabled = false;
//		
//		// Concat the allergies list
//		foodAllergiesLabel.text = "";
//		for(int i = 0; i < foodData.AllergyList.Count; i++){
//			// UNDONE Only show one allergy text and image for now, index 0
//			foodAllergiesLabel.text = foodData.AllergyList[0].ToString();
//			if(foodData.AllergyList[0] != Allergies.None){
//				allergy1Image.enabled = true;
//				allergy1Image.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
//			}
//		}
//	}
//}
