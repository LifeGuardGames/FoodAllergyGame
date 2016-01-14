using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

	public ShopEntranceUIController decoEntranceUIController;
	public ShopEntranceUIController DecoEntranceUIController {
		get { return decoEntranceUIController; }
	}
	public DinerEntranceUIController dinerEntranceUIController;
	public DinerEntranceUIController DinerEntranceUIController {
		get { return dinerEntranceUIController; }
	}

	public NewItemUIController newItemUIController;
	public NewItemUIController NewItemUIController{
		get{ return newItemUIController; }
	}

	public bool IsShopAppearHideDinerOverride = false;

	void Start(){
		
		// Refresh tier calculation
		TierManager.Instance.RecalculateTier();

		// First restaurant tutorial
		if(DataManager.Instance.GameData.Tutorial.IsTutorial1Done == false){
			decoEntranceUIController.Hide();
			unlockParent.SetActive(false); // TODO clean this up
			DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT1";
		}
		// Menu planning tutorial
		else if(DataManager.Instance.GameData.Tutorial.IsTutorial3Done == false){
			decoEntranceUIController.Hide();
			unlockParent.SetActive(true); // TODO clean this up
			DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT3";
		}
		// Default case
		else {
			// Show the deco entrance
			if(TierManager.Instance.Tier >= 3) {
				bool isFirstTimeEntrance = DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance;
				IsShopAppearHideDinerOverride = isFirstTimeEntrance;
                decoEntranceUIController.Show(isFirstTimeEntrance);
			}
			else {
				decoEntranceUIController.Hide();
			}
            unlockParent.SetActive(false); // TODO clean this up

			if(DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent) {
				DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = TierManager.Instance.GetNewEvent();
				// Lock the generate event bool until day is completed
				DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = false;
			}
		}

		// Set up pre-existing visuals and appearances for that day based on event
		PropManager.Instance.InitProps();

		//TODO Set up any new notifications here (through NotificationManager)

		// Check if tier bar needs to be updated
		if(CashManager.Instance.IsNeedToSyncTotalCash()) {
			DecoEntranceUIController.ToggleClickable(false);
			DinerEntranceUIController.ToggleClickable(false);

			int oldTotalCash = CashManager.Instance.LastSeenTotalCash;
			int newTotalCash = CashManager.Instance.TotalCash;
			NotificationQueueDataTierProgress tierNotif = new NotificationQueueDataTierProgress(SceneUtils.START, oldTotalCash, newTotalCash);
			NotificationManager.Instance.AddNotification(tierNotif);
		}

		// Check if any new deco types are unlocked at this tier
		List<string> specialItemID = TierManager.Instance.SpecialItemID;
		if(specialItemID.Count > 0){
			DecoEntranceUIController.ToggleClickable(false);
			DinerEntranceUIController.ToggleClickable(false);

			NotificationQueueDataNewItem itemNotif = new NotificationQueueDataNewItem(SceneUtils.START, specialItemID[0]);
			NotificationManager.Instance.AddNotification(itemNotif);
			TierManager.Instance.RemoveSpecialID();
        }

		// Have the spawn button see when it needs to spawn
		//		StartCoroutine(StartButtonSpawnCheck());

		// Save game data again, lock down on an event
		DataManager.Instance.SaveGameData();
		GenerateCustomerList();
		GenerateUnlockedFoodStock();
	}

	// Have the spawn button see when it needs to spawn
	//	private IEnumerator StartButtonSpawnCheck(){
	//		// Wait 2 frames for all notifications to be in
	//		yield return 0;
	//		yield return 0;
	//
	//		if(!NotificationManager.Instance.IsNotificationActive){
	//			StartButtonSpawnCallback(null, null);
	//		}
	//		else{
	//			NotificationManager.Instance.OnAllNotificationsFinished += StartButtonSpawnCallback;
	//		}
	//	}

	//	public void StartButtonSpawnCallback(object o, EventArgs e){
	//		startButton.SetActive(true);
	//	}

	//creates a list of acceptable spawning customers
	public void GenerateCustomerList() {
		DataManager.Instance.GameData.RestaurantEvent.CustomerList.Clear();
		List<ImmutableDataCustomer> customersPool = new List<ImmutableDataCustomer>();
		customersPool = DataLoaderCustomer.GetDataList();
		ImmutableDataCustomerSet currSet = DataLoaderCustomerSet.GetData(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()).CustomerSet);
		foreach (string customerID in currSet.CustomerSet) {
			if(customerID != "") {
				customersPool.Remove(DataLoaderCustomer.GetData(customerID));
			}
		}

		List<ImmutableDataCustomer> cusToDelete = new List<ImmutableDataCustomer>();
		foreach(ImmutableDataCustomer cusData in customersPool) {
			if(cusData.Tier > TierManager.Instance.Tier || cusData.Tier == -1) {
				cusToDelete.Add(cusData);
			}
		}

		foreach(ImmutableDataCustomer cusData in cusToDelete) {
			customersPool.Remove(cusData);
		}

		foreach (ImmutableDataCustomer cus in customersPool) {
			DataManager.Instance.GameData.RestaurantEvent.CustomerList.Add(cus.ID);
		}
	}

	// Given the event, generate a few set of food stocks, capped by event menussets and tier
	public void GenerateUnlockedFoodStock(){
		List<ImmutableDataFood> unlockedFoodStock = new List<ImmutableDataFood>();
		unlockedFoodStock = DataLoaderFood.GetDataList();
		// First remove all the foods that are not used for event
		ImmutableDataRemoveMenuSet currSet = DataLoaderRemoveMenuSet.GetData(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()).RemoveMenuSet);
		foreach(string foodID in currSet.RemoveMenuSet){
			if(foodID != "") {
				unlockedFoodStock.Remove(DataLoaderFood.GetData(foodID));
			}
		}

		// Take out all the foods that doesnt satisfy current tier
		List<ImmutableDataFood> foodDataToDelete = new List<ImmutableDataFood>();
		foreach(ImmutableDataFood foodData in unlockedFoodStock){
			if(foodData.Tier > TierManager.Instance.Tier){
				foodDataToDelete.Add(foodData);
			}
		}
		foreach(ImmutableDataFood foodData in foodDataToDelete){
			unlockedFoodStock.Remove(foodData);
		}

		// Populate in FoodManager for use in MenuPlanning
		FoodManager.Instance.FoodStockList = unlockedFoodStock;
	}

	public void OnPlayButtonClicked(){
		// TODO integrate with datamanager tutorial fields
		if(DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT1"
			|| DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventT0"
			|| DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventTPlayArea"
			|| DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventTFlyThru"
			|| DataManager.Instance.GameData.RestaurantEvent.CurrentEvent == "EventTVIP"){

			FoodManager.Instance.GenerateMenu(DataLoaderRemoveMenuSet.GetData("RemoveMenuSetT1").RemoveMenuSet.ToList());	// TODO remove now, fix
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT);
		}
		else{
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.MENUPLANNING, "LoadingKeyMenu", "LoadingImageMenu");
        }
	}

	public void OnComicButtonClicked(){
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
	}

	public void DecoButtonClicked(){
		DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance = false;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.DECO, "LoadingKeyDecoration");
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
