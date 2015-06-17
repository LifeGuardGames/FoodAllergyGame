using UnityEngine;
using System.Collections;

public class TempSceneChanger : MonoBehaviour {

	public void ResetGame(){
		Application.LoadLevel(SceneUtils.MENUPLANNING);
	}
}
