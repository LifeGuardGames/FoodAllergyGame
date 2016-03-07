using UnityEngine;
using UnityEngine.UI;

public class EpiPenGameUIManager : MonoBehaviour{

	public TweenToggle gameOverTween;
	public Image imgRank;
	public GameTimer tim;
	public Text timer;

	public void ShowGameOver(int attempts) {

		if(attempts == 1) {
			imgRank.sprite = SpriteCacheManager.GetChallengeButton(ChallengeReward.Gold);
		}
		else if(attempts == 2) {
			imgRank.sprite = SpriteCacheManager.GetChallengeButton(ChallengeReward.Silver);
		}
		else if(attempts == 3) {
			imgRank.sprite = SpriteCacheManager.GetChallengeButton(ChallengeReward.Bronze);
		}
		else if(attempts > 3) {
			imgRank.sprite = SpriteCacheManager.GetChallengeButton(ChallengeReward.Stone);
		}
		timer.text =  tim.counter.text;
		gameOverTween.Show();
    }

	
}
