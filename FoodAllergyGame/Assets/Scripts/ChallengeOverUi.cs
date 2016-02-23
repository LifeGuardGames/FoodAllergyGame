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
	public ChallengeProgressBarController prog;
	public int deltaCoinsAux;

	public void Populate(int negativeCash, int cashEarned, int score) {
		textPointsEarned.text = cashEarned.ToString();
		textPointsLost.text = negativeCash.ToString();
		deltaCoinsAux = score;
	}

	public void StartBar() {
		StartCoroutine("ChangePoints");
		prog.MoveBar();
	}

	public void UpdateImage(ChallengeReward rew) {
		if(rew == ChallengeReward.Silver) {
			Bronze.gameObject.SetActive(false);
			Silver.gameObject.SetActive(true);
		}
		if(rew == ChallengeReward.Bronze) {
			Stone.gameObject.SetActive(false);
			Bronze.gameObject.SetActive(true);
		}
		if(rew == ChallengeReward.Gold) {
			Silver.gameObject.SetActive(false);
			medal.gameObject.SetActive(true);
		}
	}
	private IEnumerator ChangePoints() {
		yield return new WaitForSeconds(0.5f);
		int currentCoinsAux = 0;
		int step = 1;
		while(currentCoinsAux != deltaCoinsAux) {
			if(deltaCoinsAux > 0) {
				currentCoinsAux = Mathf.Max(currentCoinsAux += step, 0);
			}
			else {
				currentCoinsAux = Mathf.Min(currentCoinsAux -= step, 0);
			}
			textScore.text = currentCoinsAux.ToString();
			// wait one frame
			yield return 0;
		}
	}
}
