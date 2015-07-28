using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StartManager : Singleton<StartManager>{

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;

	public GameObject unlockParent;

//	private ImmutableDataEvents currentEvent = null;

	void Start(){
		// Refresh tier calculation
		TierManager.Instance.RecalculateTier();

		// Check to see if the previous day has been completed
		if(DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent){
			//TODO Generate event from data
			if(DataManager.Instance.GameData.Tutorial.IsTutorial1Done == false){
				unlockParent.SetActive(false);
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT1";
			}
			else if(DataManager.Instance.GameData.Tutorial.IsTutorial2Done == false){
				unlockParent.SetActive(true);
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT2";
			}
			else{
				unlockParent.SetActive(false);
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "Event00";
			}

			// Lock the generate event bool until day is completed
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = false;
		}
		else{
//			currentEvent = DataLoaderEvents.GetData(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent);
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
		if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1"){
			FoodManager.Instance.GenerateMenu(DataLoaderMenuSet.GetData("MenuSetT1").MenuSet.ToList(), 0);
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
