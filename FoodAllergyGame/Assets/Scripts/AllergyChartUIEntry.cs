using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AllergyChartUIEntry : MonoBehaviour {

	public Image imageFood;
	public Image imageAllergy1;
	public Image imageAllergy2;
	public Image imageAllergy3;

	public void Init(string foodID){
		ImmutableDataFood foodData = DataLoaderFood.GetData(foodID);
		imageFood.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodData.SpriteName);
		if(foodData.AllergyList[0] == Allergies.None){
			imageAllergy1.gameObject.SetActive(false);
			imageAllergy2.gameObject.SetActive(false);
			imageAllergy3.gameObject.SetActive(false);
		}
		else{
			if(foodData.AllergyList.Count == 1){
				imageAllergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
				imageAllergy2.gameObject.SetActive(false);
				imageAllergy3.gameObject.SetActive(false);
			}
			else if(foodData.AllergyList.Count == 2){
				imageAllergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
				imageAllergy2.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[1]);
				imageAllergy3.gameObject.SetActive(false);
			}
			else if(foodData.AllergyList.Count == 3){
				imageAllergy1.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[0]);
				imageAllergy2.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[1]);
				imageAllergy3.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(foodData.AllergyList[2]);
			}
		}
	}
}
