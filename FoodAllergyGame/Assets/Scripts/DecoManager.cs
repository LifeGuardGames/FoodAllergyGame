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

	#region Static functions
	public static bool IsDecoBought(string decoID){
		return DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID);
	}

	public static bool IsDecoActive(string decoID){
		return DataManager.Instance.GameData.Decoration.ActiveDeco.ContainsValue(decoID);
	}
	#endregion

	void Awake(){
		sceneObjects = new Dictionary<DecoTypes, GameObject>();
		ChangeTables(SpriteCacheManager.GetDecoSpriteData(DataManager.Instance.GetActiveDecoData(DecoTypes.Table).SpriteName));
		PopulateDecoGrid();
	}

	public void PopulateDecoGrid(){

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

	public void SetDeco(string decoID){
		if(IsDecoBought(decoID)){
			// Cache local data
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
			DecoTypes decoType = decoData.Type;

			DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(decoType);
			DataManager.Instance.GameData.Decoration.ActiveDeco.Add(decoType, decoID);
			if(decoType != DecoTypes.Table && decoType != DecoTypes.Kitchen){
				ChangeItem(decoType);
			}
			else if(decoType == DecoTypes.Kitchen){
				ChangeKitchen(decoData.SpriteName, decoData.SecondarySprite);
			}
			else{
				ChangeTables(SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName));
			}
		}
		else{
			BuyItem(decoID);
		}
	}

	private void BuyItem(string decoID){
		Analytics.CustomEvent("Item Bought", new Dictionary<string, object>{
			{"Item: ", decoID}
		});
		DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, "");
		SetDeco(decoID);
	}

	private void ChangeTables(Sprite sprite){
		for(int i = 0; i < tableList.Count; i++){
			tableList[i].GetComponent<SpriteRenderer>().sprite = sprite;
		}
	}

	private void ChangeKitchen(string spriteSet, string backsprite){
		kitchenList[0].GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.GetDecoSpriteData(spriteSet);
		kitchenList[1].GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.GetDecoSpriteData(backsprite);
	}

	private void ChangeItem(DecoTypes deco){
		sceneObjects[deco].GetComponent<SpriteRenderer>().sprite = SpriteCacheManager.GetDecoSpriteData(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.ActiveDeco[deco]).SpriteName);
	}

	public void ChangeTab(string tabName){
		currentDecoPage = 0;
		currentTab = tabName;
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
