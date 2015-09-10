using UnityEngine;
using System.Collections;

// Prefab loader for the play area instead of changing sprites
public class PlayAreaLoader : MonoBehaviour {

	void Start(){
		// Delete any objects attached to this parent
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}

		if(DataManager.Instance.IsDebug){
			// Load the basic play area if debug
			LoadPrefab("PlayArea01");
		}
		else{
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(DecoTypes.PlayArea);
			if(decoData.SpriteName != "None"){
				LoadPrefab(decoData.ID);
			}
		}
	}

	private void LoadPrefab(string prefabName){
		GameObject playAreaPrefab = Resources.Load(prefabName) as GameObject;
		GameObjectUtils.AddChild(gameObject, playAreaPrefab);
	}
}
