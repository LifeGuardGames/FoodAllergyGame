using UnityEngine;
using System.Collections.Generic;

public class TableLoader : DecoLoader {
	public List<GameObject> tableParentList;
	public List<int> tableBaseSortingOrders;

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
			Table tableScript = loadedObject.GetComponent<Table>();

			if(isPlayPoof){
				ParticleUtils.PlayDecoChangePoof(tableParentList[i].transform.position);
			}


			// HACK Delete colliders while not in deco scene
			if(isDecoScene){
				loadedObject.GetComponent<Collider>().enabled = false;
				tableScript.TurnOffHighlight();
				tableScript.ToggleTableNum(false);
			}

			tableScript.TableNumber = i;     // Set index of the table number to index
			tableScript.SetBaseSortingOrder(tableBaseSortingOrders[i]);		// Set base sorting order for regular tables
        }
	}
}
