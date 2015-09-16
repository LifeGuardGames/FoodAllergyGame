using UnityEngine;
using System.Collections;

public abstract class DecoLoader : MonoBehaviour {

	protected bool isDebugEnableDeco;		// Is the deco turned on in critcal xml
	protected string debugDecoID;			// DecoID to load when debug is turned on
	private bool isDecoScene = false;				// Logic for different scenes

	void Start(){
		DecoInit();

		if(Application.loadedLevelName == SceneUtils.DECO){
			isDecoScene = true;
		}

		// Delete any objects attached to this parent
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}
		
		if(DataManager.Instance.IsDebug){
			if(isDebugEnableDeco){
				ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(debugDecoID);
				if(decoData.ButtonTitleKey != "None"){
					// Load the basic bathroom if debug
					LoadPrefab(debugDecoID);
				}
				else{
					// Dont load anything if the sprite name is None
				}
			}
			else{
				// Dont load anything if critical is not ticked on
			}
		}
		else{
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(DecoTypes.PlayArea);
			if(decoData.ButtonTitleKey != "None"){	// Dont load if it is None
				LoadPrefab(decoData.ID);
			}
		}
	}

	protected virtual void DecoInit(){}

	protected void LoadPrefab(string prefabName){
		GameObject prefab = Resources.Load(prefabName) as GameObject;
		GameObject loadedObject = GameObjectUtils.AddChild(gameObject, prefab);

		// HACK Delete colliders while not in deco scene
		if(isDecoScene){
			loadedObject.GetComponent<Collider2D>().enabled = false;
		}
	}
}
