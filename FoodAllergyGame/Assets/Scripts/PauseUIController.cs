using UnityEngine;
using System.Collections;

public class PauseUIController : MonoBehaviour {

	public TweenToggleDemux pauseMenuTween;
	public bool isActive = true;

	public void Show() {
		if(isActive) { 
			pauseMenuTween.Show();
		}
	}

	public void Hide(){
		pauseMenuTween.Hide();
	}

	public void OnResumeButton(){
		RestaurantManager.Instance.ResumeGame();
	}

	public void OnQuitButton(){
		RestaurantManager.Instance.QuitGame();
	}
}
