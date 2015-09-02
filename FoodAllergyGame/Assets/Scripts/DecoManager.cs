using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public class DecoManager : Singleton<DecoManager>{

	public List<SpriteRenderer> tableList;
	public List<SpriteRenderer> kitchenList;
	public Dictionary <DecoTypes, GameObject> sceneObjects;
	private int decoPageSize = 4;
	private int currentDecoPage = 0;
	public GameObject decoButtonPrefab;
	public Transform grid;
	private string currentTab = "Table";
	public GameObject leftButton;
	public GameObject rightButton;
	public TweenToggle decoTweenToggle;
	private List<ImmutableDataDecoItem> decoList;

	void Awake(){
		sceneObjects = new Dictionary<DecoTypes, GameObject>();
		ChangeTables(SpriteCacheManager.Instance.GetDecoSpriteData(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.currDiner[DecoTypes.Table]).SpriteName));
		PopulateDecoGrid();
	}

	public void PopulateDecoGrid(){
		currentDecoPage = 0;
		// Delete any existing buttons in the grid
		foreach(Transform child in grid){
			Destroy(child.gameObject);
		}
		//creates a list of deco based on a type, to do this the dataloader first creates the list of all the items then sorts it by type and cost before returning it
		decoList = DataLoaderDecoItem.GetDecoDataByType((DecoTypes)Enum.Parse(typeof(DecoTypes), currentTab));

		for(int i = 0; i < decoPageSize; i++){
			if(decoList.Count <= i + currentDecoPage * decoPageSize){		// Reached the end of list
				currentDecoPage--;
				break;
			}
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoList[i + currentDecoPage * decoPageSize].ID);
			GameObject decoButton = GameObjectUtils.AddChildGUI(grid.gameObject, decoButtonPrefab);
			decoButton.GetComponent<DecoButton>().Init(decoData);
			RefreshButtonShowStatus();
		}
	}

	public void ChangeSet(string key){
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(key)){
			DataManager.Instance.GameData.Decoration.currDiner.Remove(DataLoaderDecoItem.GetData(key).Type);
			DataManager.Instance.GameData.Decoration.currDiner.Add(DataLoaderDecoItem.GetData(key).Type, key);
			if(DataLoaderDecoItem.GetData(key).Type != DecoTypes.Table && DataLoaderDecoItem.GetData(key).Type != DecoTypes.Kitchen){
				ChangeItem(DataLoaderDecoItem.GetData(key).Type);
			}
			else if(DataLoaderDecoItem.GetData(key).Type == DecoTypes.Kitchen){
				ChangeKitchen(DataLoaderDecoItem.GetData(key).SpriteName, DataLoaderDecoItem.GetData(key).SecondarySprite);
			}
			else{
				ChangeTables(SpriteCacheManager.Instance.GetDecoSpriteData(DataLoaderDecoItem.GetData(key).SpriteName));
			}
		}
		else{
			BuyItem(DataLoaderDecoItem.GetData(key).Type, key);
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

	public string SetUp(DecoTypes deco){
		return DataManager.Instance.GameData.Decoration.currDiner[deco];
	}

	public void ChangeKitchen(string spriteSet, string backsprite){
		kitchenList[0].GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.instance.GetDecoSpriteData(spriteSet);
		kitchenList[1].GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.instance.GetDecoSpriteData(backsprite);
	}

	public void ChangeItem(DecoTypes deco){
		sceneObjects[deco].GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.instance.GetDecoSpriteData(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.currDiner[deco]).SpriteName);
	}

	public void ChangeTab(string tabName){
		currentTab = tabName;
		PopulateDecoGrid();
	}
	public void ChangeTab2(DecoTypes tabName){
		PopulateDecoGrid();
	}

	public void ChangePage(bool isRight){
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

	public void RefreshButtonShowStatus(){
		// Check left most limit
		if(currentDecoPage == 0){
			leftButton.SetActive(false);
		}
		else{
			leftButton.SetActive(true);
		}
		// Check right most limit
		if((currentDecoPage * 4) + 4 >= decoList.Count){
			rightButton.SetActive(false);
		}
		else{
			rightButton.SetActive(true);
		}
	}
	
	public void ToggleUI(){
		if(decoTweenToggle.IsShown){
			decoTweenToggle.Hide();
		}
		else{
			decoTweenToggle.Show();
		}
	}
}
