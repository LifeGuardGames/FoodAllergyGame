using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeProgressBarController : MonoBehaviour {

	public Image Bronze;
	public Image Silver;
	public Image Gold;


	public void MoveBar() {
		if(Bronze.fillAmount < 1.0f) {
			if(RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore() > RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.BronzeBreakPoint) {
				LeanTween.value(this.gameObject, UpdateBronzeBar, 0f, 1f, 3.0f);
			}
			else {
				LeanTween.value(this.gameObject, UpdateBronzeBar, 0f, (float)RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.BronzeBreakPoint /(float) RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore(), 3.0f);
			}
		}
		else if(Silver.fillAmount < 1.0f) {
			if(RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore() > RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.SilverBreakPoint) { 
				LeanTween.value(this.gameObject, UpdateSilverBar, 0f, 1f, 3.0f);
				}
		else {
			LeanTween.value(this.gameObject, UpdateSilverBar, 0f, (float)RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.SilverBreakPoint / (float)RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore(), 3.0f);
			}
		}
		else if(Gold.fillAmount < 1.0f) {
			if(RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore() > RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.GoldBreakPoint) {
				LeanTween.value(this.gameObject, UpdateGoldBar, 0f, 1f, 3.0f);
			}
			else {
				LeanTween.value(this.gameObject, UpdateGoldBar, 0f, (float)RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.GoldBreakPoint/ (float)RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore(), 3.0f);
			}
		}
	}

	void UpdateBronzeBar(float val) {
			Bronze.fillAmount = val;
		if(Bronze.fillAmount == 1.0f && val > 0) {
			MoveBar();
		}
	}

	void UpdateSilverBar(float val) {
		Silver.fillAmount = val;
		if(Silver.fillAmount == 1.0f && val > 0) {
			MoveBar();
		}
	}

	void UpdateGoldBar(float val) {
		Gold.fillAmount = val;
	}
}
