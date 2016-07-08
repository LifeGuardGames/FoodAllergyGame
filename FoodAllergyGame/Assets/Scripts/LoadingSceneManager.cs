using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {
	void Start () {
		//if(DataManager.Instance.GameData.Tutorial.IsComicViewed) {
		StartCoroutine(StartHelper());
		//}
		//else {
			//LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
		//}
	}

	// Wait one frame for everything to finish setting up
	public IEnumerator StartHelper() {
		yield return 0;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: false);
	}
}
