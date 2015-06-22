using UnityEngine;
using System.Collections;

public class TempSceneChanger : MonoBehaviour {

	public void ResetGame(){
		GameManager.Instance.cash = 0;
		Application.LoadLevel(SceneUtils.MENUPLANNING);
	}
}
