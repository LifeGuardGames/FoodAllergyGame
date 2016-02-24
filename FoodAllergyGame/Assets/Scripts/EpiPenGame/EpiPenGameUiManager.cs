using UnityEngine;
using UnityEngine.UI;

public class EpiPenGameUIManager : MonoBehaviour{

	public TweenToggle gameOverTween;
	public Text textGrade;
	public GameTimer tim;
	public Text timer;

	public void ShowGameOver(int attempts) {

		if(attempts == 1) {
			textGrade.text = "A";
		}
		else if(attempts == 2) {
			textGrade.text = "B";
		}
		else if(attempts == 3) {
			textGrade.text = "C";
		}
		else if(attempts > 3) {
			textGrade.text = "D";
		}
		timer.text =  tim.counter.text;
		gameOverTween.Show();
    }

	
}
