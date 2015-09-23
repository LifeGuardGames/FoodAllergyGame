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
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
	}
}
