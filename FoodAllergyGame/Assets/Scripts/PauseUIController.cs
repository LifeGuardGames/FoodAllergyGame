using UnityEngine;
using System.Collections;

public class PauseUIController : MonoBehaviour {

	public TweenToggleDemux pauseMenuTween;
	public bool isActive = true;

	public void Show() {
		if(isActive) {
			TouchManager.Instance.PauseQueue();
			pauseMenuTween.Show();
		}
	}

	public void Hide(){
		TouchManager.Instance.UnpauseQueue();
		pauseMenuTween.Hide();
	}

	public void OnResumeButton(){
		RestaurantManager.Instance.ResumeGame();
	}

	public void OnQuitButton(){
		RestaurantManager.Instance.QuitGame();
	}
}
