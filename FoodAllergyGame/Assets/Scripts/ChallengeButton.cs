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
		Debug.Log(LocalizationText.GetText("ChallengeEpiPenDesc"));
		textTitle.text = LocalizationText.GetText(challengeData.Title);
	}

	public void OnButtonClicked() {
		ChallengeMenuManager.Instance.ShowPrompt(challengeID);
	}
}
