using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVideoSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(FinishVideo());
	}
	
	IEnumerator FinishVideo() {
		yield return new WaitForSeconds(17f);
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
	}

}
