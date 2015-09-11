using UnityEngine;
using System.Collections;

public abstract class DecoLoader : MonoBehaviour {

	protected string debugDecoID;

	void Start(){
		// Delete any objects attached to this parent
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}
		
		if(DataManager.Instance.IsDebug){
			// Load the basic bathroom if debug
			LoadPrefab(debugDecoID);
		}
		else{
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(DecoTypes.PlayArea);
			if(decoData.SpriteName != "None"){
				LoadPrefab(decoData.ID);
			}
		}
		_Start();
	}

	protected virtual void _Start(){}

	protected void LoadPrefab(string prefabName){
		GameObject prefab = Resources.Load(prefabName) as GameObject;
		GameObjectUtils.AddChild(gameObject, prefab);
	}
}
