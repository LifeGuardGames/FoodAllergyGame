using UnityEngine;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour {
	void Start () {
		StartCoroutine(StartHelper());
	}

	// Wait one frame for everything to finish setting up
	public IEnumerator StartHelper() {
		yield return 0;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: false, showAdChance: false);
	}
}
