using UnityEngine;
using UnityEngine.UI;

public class EpiPenGameUIManager : MonoBehaviour{
	public TweenToggle gameOverTween;
	public Image imgRank;
	public GameTimer timer;
	public Text timerText;
	public TweenToggle checkButtonTween;

	public void CheckButtonToggle(bool isShown) {
		if(isShown) {
			checkButtonTween.Show();
		}
		else {
			checkButtonTween.Hide();
		}
	}

	public void StartGame() {
		timer.ResetTimer();
	}

	public void OnCheckButton() {
		timer.PauseTimer();
	}

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
		timerText.text = timer.Report();
		gameOverTween.Show();
    }
}
