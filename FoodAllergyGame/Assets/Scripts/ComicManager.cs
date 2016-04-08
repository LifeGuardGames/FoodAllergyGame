using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour {
	public Animator comicAnimator;

	float start;
	float end;
	float Final;

	void Start(){
		if(DataManager.Instance.GameData.Tutorial.IsComicViewed) {
			SceneManager.LoadScene(SceneUtils.START);
		}
		else {
			ComicStep(1);
		}
	}

	public void ComicStep(int step){
		switch(step){
		case 1:
			break;
		case 2:
			AudioManager.Instance.LowerBackgroundVolume(0.5f);
			AudioManager.Instance.PlayClip("ComicPage2SFX");
			break;
		case 3:
			AudioManager.Instance.PlayClip("ComicPage3SFX");
			break;
		case 4:
			DataManager.Instance.GameData.Tutorial.IsComicViewed = true;
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
			break;
		}
	}

	void OnPageStart(TimeSpan tim) {
		start = tim.Seconds;
	}

	void OnPageEnd(TimeSpan tim, int pageNum) {
		end = tim.Seconds;
		Final = end - start;
		AnalyticsManager.Instance.TimeSpentOnComicPage(Final, pageNum);
	}
}
