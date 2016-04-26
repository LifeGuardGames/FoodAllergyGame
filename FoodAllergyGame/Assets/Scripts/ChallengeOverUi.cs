using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeOverUi : MonoBehaviour {
	public Text textScore;
	public Text textPointsEarned;
	public Text textPointsLost;
	public Image trophyImage;
	public ChallengeProgressBarController progressBarController;
	public int deltaCoinsAux;

	public void Populate(int negativeCash, int cashEarned, int score) {
		textPointsEarned.text = cashEarned.ToString();
		textPointsLost.text = negativeCash.ToString();
		deltaCoinsAux = score;
	}

	public void StartBar() {
		StartCoroutine("ChangePoints");
		progressBarController.MoveBar();
	}

	public void UpdateTrophy(ChallengeReward reward) {
		trophyImage.sprite = SpriteCacheManager.GetTrophySpriteData(reward);
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
