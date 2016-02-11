using UnityEngine;

public class EpiPenGameUIManager : MonoBehaviour{

	public TweenToggle GameOverTween;

	public void ShowGameOver(int attempts) {
		Debug.Log("Game over screen");
		GameOverTween.Show();
    }
}
