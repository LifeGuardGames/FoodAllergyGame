using UnityEngine;

public class EpiPenGameUIManager : MonoBehaviour{

	public TweenToggle GameOverTween;

	public void ShowGameOver() {
		Debug.Log("Game over screen");
		GameOverTween.Show();
    }
}
