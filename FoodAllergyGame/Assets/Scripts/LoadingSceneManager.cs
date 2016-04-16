using UnityEngine;

public class LoadingSceneManager : MonoBehaviour {
	void Start () {
		if(DataManager.Instance.GameData.Tutorial.IsComicViewed) {
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
		}
		else {
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
		}
	}
}
