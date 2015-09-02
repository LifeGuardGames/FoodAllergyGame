using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SickTutorialController : MonoBehaviour {

	public Image allergyImage;
	public Image foodImage;

	public void Show(Allergies allergy, string foodSpriteName){
		allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
		foodImage.sprite = SpriteCacheManager.GetFoodSpriteData(foodSpriteName);
	}
}
