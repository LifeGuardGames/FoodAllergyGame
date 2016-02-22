using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeScoreController : MonoBehaviour {

	public Image backBar;
	public Text score;

	public void UpDateScore(int amount) {
		score.text = amount.ToString();
		backBar.sprite = SpriteCacheManager.GetChallengeButton(RestaurantManager.Instance.GetComponent<RestaurantManagerChallenge>().RewardScore());
	}
}
