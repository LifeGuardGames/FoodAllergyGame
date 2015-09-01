using UnityEngine;
using System.Collections;

public class PauseUIController : MonoBehaviour {

	public TweenToggleDemux pauseMenuTween;

	public void Show(){
		pauseMenuTween.Show();
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
