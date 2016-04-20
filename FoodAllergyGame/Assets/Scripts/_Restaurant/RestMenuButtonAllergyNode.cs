using UnityEngine;
using UnityEngine.UI;

public class RestMenuButtonAllergyNode : MonoBehaviour {
	public Image thisImage;
	public TweenToggle tweenToggle;
	private bool isUsed;

	public void Init(bool isUsed, Allergies allergyType) {
		if(!isUsed) {
			thisImage.enabled = false;
		}
		else {
			thisImage.enabled = true;
			switch(allergyType) {
				case Allergies.None:
					thisImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.None);
                    break;
				case Allergies.Dairy:
					thisImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.Dairy);
					break;
				case Allergies.Peanut:
					thisImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.Peanut);
					break;
				case Allergies.Wheat:
					thisImage.sprite = SpriteCacheManager.GetAllergySpriteData(Allergies.Wheat);
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
