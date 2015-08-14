using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecoManager : Singleton<DecoManager> {

	public List <ImmutableDataDecoItem> itemList;
	public List<GameObject> tableList;
	public Dictionary <DecoTypes, GameObject> SceneObjects;
	private int DecoPageSize = 4;
	public GameObject DecoButtonPrefab;
	public List<Transform> currentDecoSlotList;
	// Use this for initialization
	void Start () {
		itemList = DataLoaderDecoItem.GetDataList();
		PopulateDecoGrid();
	}

	public void PopulateDecoGrid(){
		itemList.Sort((x,y) => DataLoaderDecoItem.GetData(x.id).Cost.CompareTo(DataLoaderDecoItem.GetData(y.id).Cost));
		
		for(int i = 0; i < DecoPageSize; i++){
			if(itemList.Count == i){		// Reached the end of list
				break;
			}
			ImmutableDataDecoItem DecoData = DataLoaderDecoItem.GetData(itemList[i].id);
			GameObject DecoButton = GameObjectUtils.AddChildGUI(currentDecoSlotList[i].gameObject, DecoButtonPrefab);
			DecoButton.GetComponent<DecoButton>().Init(DecoData);
		}
	}
	public void ChangeSet(string ty, string id){
		DecoTypes type;
		switch(ty){
		case "Table":
			type = DecoTypes.Table;
			break;
		case "Floor":
			type = DecoTypes.Floor;
			break;
		case "Kitchen":
			type = DecoTypes.Kitchen;
			break;
		case "PlayArea":
			type = DecoTypes.PlayArea;
			break;
		case "Microwave":
			type = DecoTypes.Microwave;
			break;
		case "FlyThru":
			type = DecoTypes.FlyThru;
			break;
		default:
			type = DecoTypes.None;
			break;
		}
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(id)){
			DataManager.Instance.GameData.Decoration.currDiner.Remove(type);
			DataManager.Instance.GameData.Decoration.currDiner.Add(type,id);
		}
		else{
			BuyItem(ty,id);
		}
	}

	public void BuyItem(string ty, string decoID){
		DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, " ");
		ChangeSet(ty,decoID);
	}

	//public int getItemCost(string id){
	
	//}
}
