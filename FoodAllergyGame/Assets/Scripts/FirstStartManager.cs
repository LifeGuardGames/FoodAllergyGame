using UnityEngine;
using System.Collections;

public class FirstStartManager : Singleton<FirstStartManager> {

	public Animation restaurantAnimation;
	public GameObject button;

	public void ButtonClicked(){
		button.SetActive(false);
		restaurantAnimation.Play();
	}

	public void FinishedAnimation(){
		TransitionManager.Instance.TransitionScene(SceneUtils.COMICSCENE);
	}
}
