using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour {
	private string challengeID;

	public Text textTitle;
	public Image imageBackground;

	public void Init(ImmutableDataChallenge challengeData) {
		gameObject.name = challengeData.ID;
		challengeID = challengeData.ID;
		ChallengeReward rewardProgress = DataManager.Instance.GameData.Challenge.ChallengeProgress[challengeID];
		imageBackground.sprite = SpriteCacheManager.GetChallengeButton(rewardProgress);

		// TODO Insert text here
		textTitle.text = challengeData.Title;
	}

	public void OnButtonClicked() {
		ChallengeMenuManager.Instance.ShowPrompt(challengeID);
	}
}
