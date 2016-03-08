using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour {
	public GameObject canvas;
	private GameObject currScene;
	private GameObject comicPage;

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
			currScene = Resources.Load("ComicPage1")as GameObject;
			comicPage = GameObjectUtils.AddChildGUI(canvas, currScene);
			comicPage.GetComponentInChildren<Button>().onClick.AddListener(() => ComicStep(2));
			break;
		case 2:
			AudioManager.Instance.LowerBackgroundVolume(0.5f);
			AudioManager.Instance.PlayClip("ComicPage2SFX");
			Destroy(comicPage);
			currScene = Resources.Load("ComicPage2")as GameObject;
			comicPage = GameObjectUtils.AddChildGUI(canvas, currScene);
			comicPage.GetComponentInChildren<Button>().onClick.AddListener(() => ComicStep(3));
			break;
		case 3:
			AudioManager.Instance.PlayClip("ComicPage3SFX");
			Destroy(comicPage);
			currScene = Resources.Load("ComicPage3")as GameObject;
			comicPage = GameObjectUtils.AddChildGUI(canvas, currScene);
			comicPage.GetComponentInChildren<Button>().onClick.AddListener(() => ComicStep(4));
			break;
		case 4:
			DataManager.Instance.GameData.Tutorial.IsComicViewed = true;
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
			break;
		}
	}
}
