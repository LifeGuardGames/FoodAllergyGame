using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StartManager : Singleton<StartManager>{

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;

	private ImmutableDataEvents currentEvent = null;

	void Start(){
		// Refresh tier calculation
		TierManager.Instance.RecalculateTier();

		// Check to see if the previous day has been completed
		if(DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent){
			//TODO Generate event from data
//			currentEvent = ....
			if(DataManager.Instance.GetEvent()!="EventT2"){
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = DataLoaderEvents.GetData("Event0T").ID;
			}
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = false;
		}
		else{
			currentEvent = DataLoaderEvents.GetData(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent);
		}

		//TODO Set up visuals and appearances for that day based on event
	}

	public void GenerateUnlockedFoodStock(){
		List<ImmutableDataFood> foodStock = new List<ImmutableDataFood>(DataLoaderFood.GetDataList());
		List<string> unlockedFoodStock = new List<string>();
		for (int i = 0; i < foodStock.Count; i++){
			if(foodStock[i].Tier <= TierManager.Instance.Tier){
				unlockedFoodStock.Add(foodStock[i].ID);
			}
		}
		ImmutableDataMenuSet currSet = DataLoaderMenuSet.GetData(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()).MenuSet);
		for (int i = 0; i < currSet.MenuSet.Length; i++){
			unlockedFoodStock.Remove(currSet.MenuSet[i]);
		}
		DataManager.Instance.GameData.RestaurantEvent.MenuPlanningStock = unlockedFoodStock;
	}

	public void OnPlayButtonClicked(){
		// TODO integrate with datamanager tutorial fields
		if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "Event0T"){
			FoodManager.Instance.GenerateMenu(DataLoaderMenuSet.GetData("MenuSet0T").MenuSet.ToList(), 0);
			TransitionManager.Instance.TransitionScene(SceneUtils.TUTSCENE);
		}
		else{
			TransitionManager.Instance.TransitionScene(SceneUtils.MENUPLANNING);
		}
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
