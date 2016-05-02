using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class DecoLoader : MonoBehaviour {

	protected bool isDebugEnableDeco;		// Is the deco turned on in critcal xml
	protected string debugDecoID;			// DecoID to load when debug is turned on
	protected DecoTypes decoType;			// Type of decoration this is
	protected GameObject loadedObject;		// Aux for post load
	protected bool isDecoScene = false;		// Logic for different scenes

	protected virtual void DecoInit(){}

	// If the deco scene, load by itself
	void Start(){
		if(SceneManager.GetActiveScene().name == SceneUtils.DECO){
			DecoInit(); // Initialize all type specific variables in children
			isDecoScene = true;
			DecoManager.Instance.DecoLoaderHash.Add(decoType, this);
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(decoType);
			LoadDeco(decoData);
		}
	}
	// now handles deco init in the resturant scene so we can control when the deco is initialized
	public void ResturantInit() {
		DecoInit();
		if(DataManager.Instance.IsDebug) {
			if(isDebugEnableDeco) {
				ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(debugDecoID);
				LoadDeco(decoData);
			}
			else {
				// Dont load anything if critical is not ticked on
			}
		}
		else {
			if(RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>() == null) {
				ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(decoType);
				LoadDeco(decoData);
			}
			else {
				if(decoType == DecoTypes.PlayArea || decoType == DecoTypes.VIP || decoType == DecoTypes.FlyThru) {
					LoadDeco(null);
				}
				else {
					ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(decoType);
					LoadDeco(decoData);
				}
			}
		}
	}

	// Load the deco, accounts for null, deletes existing children as well
	public virtual void LoadDeco(ImmutableDataDecoItem decoData, bool isPlayPoof = false){
		// Delete any objects attached to this parent
		foreach(Transform child in transform){
			Destroy(child.gameObject);
		}

		if(isPlayPoof){
			ParticleAndFloatyUtils.PlayDecoChangePoof(transform.position);
		}

		// Load decos if there is any
		if(decoData != null){
			GameObject prefab = Resources.Load(decoData.ID) as GameObject;
			loadedObject = GameObjectUtils.AddChild(gameObject, prefab);

			// HACK Disable colliders while not in deco scene
			if(isDecoScene){
				loadedObject.GetComponent<Collider>().enabled = false;
            }
		}
	}
}
