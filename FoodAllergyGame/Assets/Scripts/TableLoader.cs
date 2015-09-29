using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableLoader : DecoLoader {
	public List<GameObject> tableParentList;

	protected override void DecoInit(){
		isDebugEnableDeco = true;	// Always on
		debugDecoID = Constants.GetDebugConstant<string>("TableID");
		decoType = DecoTypes.Table;
	}

	// Overrided parent function, special case
	public override void LoadDeco(ImmutableDataDecoItem decoData, bool isPlayPoof = false){
		for(int i = 0; i < tableParentList.Count; i++){
			// Delete any objects attached to each parent
			foreach(Transform child in tableParentList[i].transform){
				Destroy(child.gameObject);
			}
			GameObject prefab = Resources.Load(decoData.ID) as GameObject;
			loadedObject = GameObjectUtils.AddChild(tableParentList[i], prefab);

			if(isPlayPoof){
				ParticleUtils.PlayDecoChangePoof(tableParentList[i].transform.position);
			}

			// HACK Delete colliders while not in deco scene
			if(isDecoScene){
				loadedObject.GetComponent<Collider>().enabled = false;
			}

			// Set the index of the table number to index
			loadedObject.GetComponent<Table>().TableNumber = i;
		}
	}
}
