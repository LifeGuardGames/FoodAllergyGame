﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour {
	public Animator comicAnimator;
	public TweenToggle fadeTween;

	private int nextStepAux = 0;

	private float start;
	private float end;
	private float final;

	void Start(){
		if(DataManager.Instance.GameData.Tutorial.IsComicViewed) {
			SceneManager.LoadScene(SceneUtils.START);
		}
		else {
			AnalyticsManager.Instance.TutorialFunnel("Started Coimic");
			ComicStep(1);
		}
	}

	public void FadePlayNextStep(int step) {
		nextStepAux = step;
        fadeTween.Show();
    }

	public void OnFadeShowComplete() {
		if(nextStepAux != 0) {
			ComicStep(nextStepAux);
		}
		
		if(nextStepAux != 4) {      // Dont fade for the last step
			fadeTween.Hide();
		}
	}

	private void ComicStep(int step){
		switch(step){
		case 1:
			comicAnimator.Play("ComicPage1");
            break;
		case 2:
			comicAnimator.Play("ComicPage2");
			break;
		case 3:
			comicAnimator.Play("ComicPage3");
			break;
		case 4:
			AnalyticsManager.Instance.TutorialFunnel("Finished Coimic");
			DataManager.Instance.GameData.Tutorial.IsComicViewed = true;
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
			break;
		}
	}

	private void OnPageStart(TimeSpan tim) {
		start = tim.Seconds;
	}

	private void OnPageEnd(TimeSpan tim, int pageNum) {
		end = tim.Seconds;
		final = end - start;
		AnalyticsManager.Instance.TimeSpentOnComicPage(final, pageNum);
	}
}
