using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeOverUi : MonoBehaviour {


	public Text textCustomersMissed;
	public Text textScore;
	public Text textCashEarned;
	public Text textEarningsNet;

	public void Populate(int customersMissed, int cashEarned, int score) {
		textCustomersMissed.text = customersMissed.ToString();
		textScore.text = score.ToString();
		textCashEarned.text = cashEarned.ToString();
	}
}
