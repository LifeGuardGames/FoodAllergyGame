using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Analytics;

public class StartManager : Singleton<StartManager>{
	public GameObject sceneObjectParent;
	public GameObject SceneObjectParent{
		get{ return sceneObjectParent; }
	}

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;
	public AlphaTweenToggle infoFadeBackground;

	public GameObject unlockParent;
	public DecoEntranceUIController decoEntranceUIController;
	public NewItemUIController newItemUIController;
	public NewItemUIController NewItemUIController{
		get{ return newItemUIController; }
	}

	void Start(){
		// Refresh tier calculation
		TierManager.Instance.RecalculateTier();

		// Check to see if the previous day has been completed
		if(DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent){
			if(DataManager.Instance.GameData.Tutorial.IsTutorial1Done == false){
				decoEntranceUIController.Hide();
				unlockParent.SetActive(false); // TODO clean this up
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT1";
			}
			else if(DataManager.Instance.GameData.Tutorial.IsTutorial3Done == false){
				decoEntranceUIController.Hide();
				unlockParent.SetActive(true); // TODO clean this up
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT3";
			}
			else if(DataManager.Instance.GameData.Tutorial.IsSpecialDecoTutDone == false){
				decoEntranceUIController.Hide();
			}
			else{
				Debug.Log (DataManager.Instance.GameData.Tutorial.IsSpecialDecoTutDone);
				// Show the deco entrance
				bool isFirstTimeEntrance = DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance;
				decoEntranceUIController.Show(isFirstTimeEntrance);

				unlockParent.SetActive(false); // TODO clean this up
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = TierManager.Instance.GetNewEvent();
			}

			// Lock the generate event bool until day is completed
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = false;

			// Save game data again, lock down on an event

		}

		//TODO Set up pre-existing visuals and appearances for that day based on event


		//TODO Set up any new notifications here (through NotificationManager)

		// Check if tier bar needs to be updated
//		if(DataManager.Instance.GameData.Cash.IsNeedToSyncTotalCash){
//			int oldTotalCash = DataManager.Instance.GameData.Cash.LastSeenTotalCash;
//			int newTotalCash = DataManager.Instance.GameData.Cash.TotalCash;
//			NotificationQueueDataTierProgress tierNotif = new NotificationQueueDataTierProgress(SceneUtils, oldTotalCash, newTotalCash);
//			NotificationManager.Instance.AddNotification(tierNotif);
//		}

		// Check if any new deco types are unlocked at this tier
		string specialItemID = TierManager.Instance.SpecialItemID;
		if(!string.IsNullOrEmpty(specialItemID)){
			NotificationQueueDataNewItem itemNotif = new NotificationQueueDataNewItem(SceneUtils.START, specialItemID);
			NotificationManager.Instance.AddNotification(itemNotif);
		}

		DataManager.Instance.SaveGameData();
		GenerateUnlockedFoodStock();
	}

	// Finished event from NotificationQueueDataNewItem notification
	public void SyncLastSeenTotalCash(){
		DataManager.Instance.GameData.Cash.SyncLastSeenTotalCash();
	}

	// Given the event, generate a few set of food stocks, capped by event menussets and tier
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
			if(foodData.Tier > TierManager.Instance.Tier){
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
		if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1" ||DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT0" || DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventTP"|| DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventTF"|| DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventTV"){
			FoodManager.Instance.GenerateMenu(DataLoaderMenuSet.GetData("MenuSetT1").MenuSet.ToList(), 0);
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT);
		}
		else{
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.MENUPLANNING);
		}
	}

	public void OnComicButtonClicked(){
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
	}

	public void DecoButtonClicked(){
		DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance = false;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.DECO);
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
