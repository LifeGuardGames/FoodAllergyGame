using System;
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

	public void SkipComic(int page) {
		AnalyticsManager.Instance.SkipComic(page);
		DataManager.Instance.GameData.Tutorial.IsComicViewed = true;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
	}

	private void ComicStep(int step){
		switch(step){
		case 1:
			comicAnimator.Play("ComicPage1");
			AnalyticsManager.Instance.TutorialFunnel("Comic Page 1");
			break;
		case 2:
			comicAnimator.Play("ComicPage2");
			AnalyticsManager.Instance.TutorialFunnel("Comic Page 2");
			break;
		case 3:
			comicAnimator.Play("ComicPage3");
			AnalyticsManager.Instance.TutorialFunnel("Comic Page 3");
			break;
		case 4:
			AnalyticsManager.Instance.TutorialFunnel("Finished Comic");
			DataManager.Instance.GameData.Tutorial.IsComicViewed = true;
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
			break;
		}
	}
}
