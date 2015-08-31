using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class DecoManager : Singleton<DecoManager>{

	public List <ImmutableDataDecoItem> itemList;
	public List<SpriteRenderer> tableList;
	public List<SpriteRenderer> KitchenList;
	public Dictionary <DecoTypes, GameObject> SceneObjects;
	private int DecoPageSize = 4;
	public int currentDecoPage = 0;
	public GameObject DecoButtonPrefab;
	public Transform grid;
	public string currentTab = "Table";
	public TweenToggle decoTweenToggle;

	void Awake(){
		itemList = DataLoaderDecoItem.GetDataList();
		SceneObjects = new Dictionary<DecoTypes, GameObject>();
		ChangeTables(Resources.Load <Sprite>(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.currDiner[DecoTypes.Table]).SpriteName));
		PopulateDecoGrid();
	}

	public void PopulateDecoGrid(){
		// Delete any existing buttons in the grid
		foreach(Transform child in grid){
			Destroy(child.gameObject);
		}

		List<ImmutableDataDecoItem> decoList = new List<ImmutableDataDecoItem>();
		for(int i = 0; i < itemList.Count; i++){
			if(itemList[i].Type.ToString() == currentTab){
				decoList.Add(itemList[i]);
			}
		}
		decoList.Sort((x,y) => DataLoaderDecoItem.GetData(x.ID).Cost.CompareTo(DataLoaderDecoItem.GetData(y.ID).Cost));

		for(int i = 0; i < DecoPageSize; i++){
			if(decoList.Count <= i + currentDecoPage * DecoPageSize){		// Reached the end of list
				currentDecoPage--;
				break;
			}
			ImmutableDataDecoItem DecoData = DataLoaderDecoItem.GetData(decoList[i + currentDecoPage * DecoPageSize].ID);
			GameObject DecoButton = GameObjectUtils.AddChildGUI(grid.gameObject, DecoButtonPrefab);
			DecoButton.GetComponent<DecoButton>().Init(DecoData);
		}
	}

	public void ChangeSet(string id){
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(id)){
			DataManager.Instance.GameData.Decoration.currDiner.Remove(DataLoaderDecoItem.GetData(id).Type);
			DataManager.Instance.GameData.Decoration.currDiner.Add(DataLoaderDecoItem.GetData(id).Type, id);
			if(DataLoaderDecoItem.GetData(id).Type != DecoTypes.Table && DataLoaderDecoItem.GetData(id).Type != DecoTypes.Kitchen){
				ChangeItem(DataLoaderDecoItem.GetData(id).Type);
			}
			else if(DataLoaderDecoItem.GetData(id).Type == DecoTypes.Kitchen){
				ChangeKitchen(DataLoaderDecoItem.GetData(id).SpriteName, DataLoaderDecoItem.GetData(id).SecondarySprite);
			}
			else{
				ChangeTables(Resources.Load<Sprite>(DataLoaderDecoItem.GetData(id).SpriteName));
			}
		}
		else{
			BuyItem(DataLoaderDecoItem.GetData(id).Type, id);
		}
	}

	public void BuyItem(DecoTypes ty, string decoID){
		Analytics.CustomEvent("Item Bought", new Dictionary<string, object>{
			{"Item: ", decoID}
		});
		DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, " ");
		ChangeSet(decoID);
	}

	public void ChangeTables(Sprite sprite){
		for(int i = 0; i < tableList.Count; i++){
			tableList[i].GetComponent<SpriteRenderer>().sprite = sprite;
		}
	}

	public string setUp(DecoTypes deco){
		return DataManager.Instance.GameData.Decoration.currDiner[deco];
	}

	public void ChangeKitchen(string SpriteSet, string backsprite){
		KitchenList[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpriteSet);
		KitchenList[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(backsprite);
	}

	public void ChangeItem(DecoTypes deco){
		SceneObjects[deco].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.currDiner[deco]).SpriteName);
	}

	public void ChangeTab(string tabName){
		currentTab = tabName;
		PopulateDecoGrid();
	}
	public void ChangeTab2(DecoTypes tabName){
		PopulateDecoGrid();
	}

	public void changePage(bool isRight){
		if(isRight){
			currentDecoPage++;
		}
		else{
			if(currentDecoPage > 0){
				currentDecoPage--;
			}
		}
		PopulateDecoGrid();
	}

	//public int getItemCost(string id){
	
	//}
	
	public void ToggleUI(){
		if(decoTweenToggle.IsShown){
			decoTweenToggle.Hide();
		}
		else{
			decoTweenToggle.Show();
		}
	}
}
