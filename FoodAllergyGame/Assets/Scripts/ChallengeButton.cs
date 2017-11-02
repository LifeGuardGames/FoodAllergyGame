using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour {
	private string challengeID;
	public Image Completed;

	public void Init(ImmutableDataChallenge challengeData, int challengeCount) {
		//if(challengeData.ChallengeType != ChallengeTypes.Character) {
			//line.rectTransform.SetParent(GameObject.Find("Line Holder").transform);
		//}
		gameObject.name = challengeData.ID;
		gameObject.transform.position = new Vector2(gameObject.transform.position.x + (350* challengeCount), gameObject.transform.position.y);
		challengeID = challengeData.ID;
		ChallengeReward rewardProgress = DataManager.Instance.GameData.Challenge.ChallengeProgress[challengeID];
		switch(rewardProgress) {
			case ChallengeReward.Bronze:
				Completed.sprite = Resources.Load<Sprite>("ChallengeButtonBronze") as Sprite;
				Completed.gameObject.SetActive(true);
				break;
			case ChallengeReward.Silver:
				Completed.sprite = Resources.Load<Sprite>("ChallengeButtonSilver") as Sprite;
				Completed.gameObject.SetActive(true);
				break;
			case ChallengeReward.Gold:
				Completed.sprite = Resources.Load<Sprite>("ChallengeButtonGold") as Sprite;
				Completed.gameObject.SetActive(true);
				break;
			}	
		}


	public void OnButtonClicked() {
		ChallengeMenuManager.Instance.ShowPrompt(challengeID);
		AudioManager.Instance.PlayClip("ChallengePick");
	}
}
