using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(DataManager.Instance.GameData.Tutorial.IsComicViewed) {
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
		}
		else {
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
		}
	}
	
}
