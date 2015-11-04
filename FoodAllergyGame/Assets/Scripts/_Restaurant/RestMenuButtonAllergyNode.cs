using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestMenuButtonAllergyNode : MonoBehaviour {
	public Image thisImage;
	public GameObject childObject;
	public Image allergyImage;
	public TweenToggle tweenToggle;
	private bool isUsed;

	public void Init(bool isUsed, Allergies allergyType) {
		if(!isUsed) {
			thisImage.enabled = false;
			childObject.SetActive(false);
		}
		else {
			thisImage.enabled = true;
			childObject.SetActive(true);
			switch(allergyType) {
				case Allergies.None:
					allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.None);
                    break;
				case Allergies.Dairy:
					allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.Dairy);
					break;
				case Allergies.Peanut:
					allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.Peanut);
					break;
				case Allergies.Wheat:
					allergyImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.Wheat);
					break;
				default:
					Debug.Log("Error enum " + allergyType.ToString());
					break;
			}
		}
    }

	public void Show() {
		tweenToggle.Show();
    }

	public void Hide() {
		tweenToggle.Hide();
    }
}
