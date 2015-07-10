using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComicManager : MonoBehaviour {
	public GameObject canvas;
	private GameObject currScene;
	private GameObject comicPage;

	void Start(){
		ComicStep(1);
	}

	public void ComicStep(int step){
		switch(step){
		case 1:
			currScene = Resources.Load("ComicPage1")as GameObject;
			comicPage = GameObjectUtils.AddChildGUI(canvas, currScene);
			comicPage.GetComponentInChildren<Button>().onClick.AddListener(() => ComicStep(2));
			AudioManager.Instance.PlayClip("customerEnter");
			break;
		case 2:
			Destroy(comicPage);
			currScene = Resources.Load("ComicPage2")as GameObject;
			comicPage = GameObjectUtils.AddChildGUI(canvas, currScene);
			comicPage.GetComponentInChildren<Button>().onClick.AddListener(() => ComicStep(3));
			AudioManager.Instance.PlayClip("pop");
			break;
		case 3:
			Destroy(comicPage);
			currScene = Resources.Load("ComicPage3")as GameObject;
			comicPage = GameObjectUtils.AddChildGUI(canvas, currScene);
			comicPage.GetComponentInChildren<Button>().onClick.AddListener(() => ComicStep(4));
			AudioManager.Instance.PlayClip("readingMenu");
			break;
		case 4:
			Application.LoadLevel(SceneUtils.START);
			break;
		}
	}
}
