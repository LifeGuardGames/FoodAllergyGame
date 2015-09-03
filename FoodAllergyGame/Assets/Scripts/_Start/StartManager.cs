using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Analytics;

public class StartManager : Singleton<StartManager>{

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;
	public AlphaTweenToggle infoFadeBackground;

	public GameObject unlockParent;
	public DecoEntranceUIController decoEntranceUIController;

//	private ImmutableDataEvents currentEvent = null;

	void Start(){
		// Refresh tier calculation
		TierManager.Instance.RecalculateTier();
		if(Constants.GetDebugConstant<string>("EventID") != default(string)){
			DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = Constants.GetDebugConstant<string>("EventID");
		}
		// Check to see if the previous day has been completed
		else if(DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent){
			if(DataManager.Instance.GameData.Tutorial.IsTutorial1Done == false){
				decoEntranceUIController.Hide();
				unlockParent.SetActive(false); // TODO clean this up
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT1";
			}
			else if(DataManager.Instance.GameData.Tutorial.IsTutorial2Done == false){
				decoEntranceUIController.Hide();
				unlockParent.SetActive(true); // TODO clean this up
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT3";
			}
			else{
				// Show the deco entrance
				bool isFirstTimeEntrance = DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance;
				decoEntranceUIController.Show(isFirstTimeEntrance);

				unlockParent.SetActive(false); // TODO clean this up
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = TierManager.Instance.GetNewEvent();
			}

			// Lock the generate event bool until day is completed
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = false;

			// Save game data again, lock down on an event
			DataManager.Instance.SaveGameData();
		}
		else{
//			currentEvent = DataLoaderEvents.GetData(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent);
		}

		//TODO Set up visuals and appearances for that day based on event

		GenerateUnlockedFoodStock();
	}

	public void GenerateUnlockedFoodStock(){
		List<ImmutableDataFood> unlockedFoodStock = new List<ImmutableDataFood>();

		// First add all the foods that are used for event
		ImmutableDataMenuSet currSet = DataLoaderMenuSet.GetData(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()).MenuSet);
		foreach(string foodID in currSet.MenuSet){
			unlockedFoodStock.Add(DataLoaderFood.GetData(foodID));
		}

		// Take out all the foods that doesnt satisfy current tier
		List<ImmutableDataFood> foodDataToDelete = new List<ImmutableDataFood>();
		foreach(ImmutableDataFood foodData in unlockedFoodStock){
			if(foodData.Tier < TierManager.Instance.Tier){
				foodDataToDelete.Add(foodData);
			}
		}
		foreach(ImmutableDataFood foodData in foodDataToDelete){
			Debug.LogWarning("Removing food due to tier limit!");	// TODO just testing down the road to see if this works
			unlockedFoodStock.Remove(foodData);
		}

		// Populate in FoodManager for use in MenuPlanning
		FoodManager.Instance.FoodStockList = unlockedFoodStock;
	}

	public void OnPlayButtonClicked(){
		// TODO integrate with datamanager tutorial fields
		if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1" ||DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT0"){
			FoodManager.Instance.GenerateMenu(DataLoaderMenuSet.GetData("MenuSetT1").MenuSet.ToList(), 0);
			TransitionManager.Instance.TransitionScene(SceneUtils.RESTAURANT);
		}
		else{
			TransitionManager.Instance.TransitionScene(SceneUtils.MENUPLANNING);
		}
	}

	public void OnComicButtonClicked(){
		TransitionManager.Instance.TransitionScene(SceneUtils.COMICSCENE);
	}

	public void DecoButtonClicked(){
		DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance = false;
		TransitionManager.Instance.TransitionScene(SceneUtils.DECO);
	}

	public void OnInfoButtonClicked(){

	}

	public void ShowStartDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Hide();
		startDemux.Show();
		infoFadeBackground.Hide();
	}
	
	public void ShowInfoCategoriesDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Show();
		startDemux.Hide();
		InfoManager.Instance.ClearDetail();
		infoFadeBackground.Show();
	}
	
	public void ShowInfoDetailDemux(){
		infoDetailDemux.Show();
		infoCategoriesDemux.Hide();
		startDemux.Hide();
	}
}
