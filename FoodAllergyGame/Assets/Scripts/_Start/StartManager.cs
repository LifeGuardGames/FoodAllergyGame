using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartManager : Singleton<StartManager>{

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;

	void Start(){
		//TODO Generate event from data
		//DataManager.Instance.SetEvent( "Event0" + UnityEngine.Random.Range(0,6).ToString());
		//TODO Set up visuals and appearances for that day		
	}

	public void GenerateUnlockedFoodStock(){
		List<ImmutableDataFood> foodStock = new List<ImmutableDataFood>(DataLoaderFood.GetDataList());
		List<string> unlockedFoodStock = new List<string>();
		for (int i = 0; i < foodStock.Count; i++){
			if(foodStock[i].Tier <= calculateTier()){
				unlockedFoodStock.Add(foodStock[i].ID);
			}
		}
		ImmutableDataMenuSet currSet = DataLoaderMenuSet.GetData(DataLoaderEvents.GetData(DataManager.instance.GetEvent()).MenuSet);
		for (int i = 0; i < currSet.menuSet.Length; i++){
			unlockedFoodStock.Remove(currSet.menuSet[i]);
		}
		DataManager.Instance.GameData.RestaurantEvent.menuPlanningStock = unlockedFoodStock;
	}

	public int calculateTier(){
		if(DataManager.Instance.GameData.Cash.TotalCash < 700){
			return 0;
		}
		else if (DataManager.Instance.GameData.Cash.TotalCash < 1400){
			return 1;
		}
		else if (DataManager.Instance.GameData.Cash.TotalCash < 2400){
			return 2;
		}
		else if (DataManager.Instance.GameData.Cash.TotalCash < 3600){
			return 4;
		}
//		else if (DataManager.Instance.GameData.Cash.TotalCash < 5000){
//			return 5;
//		}
		else{
			return 0;
		}

	}

	public void OnPlayButtonClicked(){
		TransitionManager.Instance.TransitionScene(SceneUtils.MENUPLANNING);
	}

	public void OnComicButtonClicked(){
		TransitionManager.Instance.TransitionScene(SceneUtils.COMICSCENE);
	}

	public void OnDecoButtonClicked(){

	}

	public void OnInfoButtonClicked(){

	}

	public void ShowStartDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Hide();
		startDemux.Show();
	}
	
	public void ShowInfoCategoriesDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Show();
		startDemux.Hide();
		InfoManager.Instance.ClearDetail();
	}
	
	public void ShowInfoDetailDemux(){
		infoDetailDemux.Show();
		infoCategoriesDemux.Hide();
		startDemux.Hide();
	}
}
