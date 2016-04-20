using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour {
	private string challengeID;

	public Text textTitle;
	public Image imageBackground;
	public Image imageCore;

	public void Init(ImmutableDataChallenge challengeData) {
		gameObject.name = challengeData.ID;
		challengeID = challengeData.ID;
		ChallengeReward rewardProgress = DataManager.Instance.GameData.Challenge.ChallengeProgress[challengeID];
		imageBackground.sprite = SpriteCacheManager.GetChallengeButton(rewardProgress);
		textTitle.text = LocalizationText.GetText(challengeData.Title);
		if(challengeData.IsBossChallenge) {
			imageCore.gameObject.SetActive(true);
			// TODO abtract out this definition?
			if(rewardProgress == ChallengeReward.Bronze
				|| rewardProgress == ChallengeReward.Silver
				|| rewardProgress == ChallengeReward.Gold) {
				
			}
			else {
				imageCore.color = new Color(0f, 0f, 0f, 1f);
			}
		}
		else {
			imageCore.gameObject.SetActive(false);
		}
	}

	public void OnButtonClicked() {
		ChallengeMenuManager.Instance.ShowPrompt(challengeID);
	}
}
