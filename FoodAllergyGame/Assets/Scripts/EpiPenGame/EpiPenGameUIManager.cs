using UnityEngine;
using UnityEngine.UI;

public class EpiPenGameUIManager : MonoBehaviour{
	public TweenToggleDemux gameOverTween;
	public Image imgRank;
	public GameTimer timer;
	public Text timerText;
	public TweenToggle checkButtonTween;
	public TweenToggle fadePanel;
	public ParticleSystem confettiParticle;

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

	public void ContinueTimer() {
		timer.ContinueTimer();
	}

	public void OnCheckButton() {
		timer.PauseTimer();
	}

	public void FadeToggle(bool isShown) {
		if(isShown) {
			fadePanel.Show();
		}
		else {
			fadePanel.Hide();
		}
    }

	public void ShowGameOver(int attempts) {
		if(attempts == 1) {
			imgRank.sprite = SpriteCacheManager.GetTrophySpriteData(ChallengeReward.Gold);
			if(DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] != ChallengeReward.Gold) {
				DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] = ChallengeReward.Gold;
            }
			confettiParticle.Play();
		}
		else if(attempts == 2) {
			imgRank.sprite = SpriteCacheManager.GetTrophySpriteData(ChallengeReward.Silver);
			if(DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] != ChallengeReward.Silver || DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] != ChallengeReward.Gold) {
				DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] = ChallengeReward.Silver;
			}
			confettiParticle.Play();
		}
		else if(attempts == 3) {
			imgRank.sprite = SpriteCacheManager.GetTrophySpriteData(ChallengeReward.Bronze);
			if(DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] != ChallengeReward.Silver || DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] != ChallengeReward.Gold) {
				DataManager.Instance.GameData.Challenge.ChallengeProgress["Challenge00"] = ChallengeReward.Bronze;
			}
			confettiParticle.Play();
		}
		else if(attempts > 3) {
			imgRank.sprite = SpriteCacheManager.GetTrophySpriteData(ChallengeReward.Stone);
		}
		timerText.text = timer.Report();
		gameOverTween.Show();
		AudioManager.Instance.PlayClip("EndOfDay");
	}
}
