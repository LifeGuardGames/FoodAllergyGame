using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SickTutorialController : MonoBehaviour {

	public Image allergyImage;
	public Image foodImage;

	public void Show(Allergies allergy, string foodSpriteName){
		allergyImage.sprite = SpriteCacheManager.Instance.GetAllergySpriteData(allergy);
		foodImage.sprite = SpriteCacheManager.Instance.GetFoodSpriteData(foodSpriteName);
	}
}
