using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeOverUi : MonoBehaviour {


	//public Text textCustomersMissed;
	public Text textScore;
	public Text textPointsEarned;
	public Text textPointsLost;
	public Image medal;
	public Image Silver;
	public Image Bronze;
	public Image Stone;

	public void Populate(int negativeCash, int cashEarned, int score, ChallengeReward rew) {
		textScore.text = score.ToString();
		textPointsEarned.text = cashEarned.ToString();
		textPointsLost.text = negativeCash.ToString();
		if(rew == ChallengeReward.Silver) {
			Stone.gameObject.SetActive(false);
			Silver.gameObject.SetActive(true);
		}
		if(rew == ChallengeReward.Bronze) {
			Stone.gameObject.SetActive(false);
			Bronze.gameObject.SetActive(true);
		}
		if(rew == ChallengeReward.Gold) {
			Stone.gameObject.SetActive(false);
			medal.gameObject.SetActive(true);
		}

	}
}
