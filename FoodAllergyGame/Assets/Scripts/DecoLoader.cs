using UnityEngine;
using System.Collections;

public abstract class DecoLoader : MonoBehaviour {

	protected bool isDebugEnableDeco;		// Is the deco turned on in critcal xml
	protected string debugDecoID;			// DecoID to load when debug is turned on
	protected DecoTypes decoType;			// Type of decoration this is
	protected GameObject loadedObject;		// Aux for post load
	protected bool isDecoScene = false;		// Logic for different scenes

	protected virtual void DecoInit(){}

	void Start(){
		DecoInit();	// Initialize all type specific variables in children

		if(Application.loadedLevelName == SceneUtils.DECO){
			isDecoScene = true;
			DecoManager.Instance.DecoLoaderHash.Add(decoType, this);
		}
		
		if(DataManager.Instance.IsDebug){
			if(isDebugEnableDeco){
				ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(debugDecoID);
				LoadDeco(decoData);
			}
			else{
				// Dont load anything if critical is not ticked on
			}
		}
		else{
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(decoType);
			LoadDeco(decoData);
		}
	}

	// Load the deco, accounts for "None" as well, deletes the parent's children as well
	public virtual void LoadDeco(ImmutableDataDecoItem decoData){
		// Floors loads textures
		if(decoData.Type == DecoTypes.Floor){

		}
		// Everything else loads prefabs
		else{
			// Delete any objects attached to this parent
			foreach(Transform child in transform){
				Destroy(child.gameObject);
			}

			if(decoData.ButtonTitleKey != "None"){
				GameObject prefab = Resources.Load(decoData.ID) as GameObject;
				loadedObject = GameObjectUtils.AddChild(gameObject, prefab);
			
				// HACK Delete colliders while not in deco scene
				if(isDecoScene){
					loadedObject.GetComponent<Collider>().enabled = false;
				}
			}
		}
	}
}
