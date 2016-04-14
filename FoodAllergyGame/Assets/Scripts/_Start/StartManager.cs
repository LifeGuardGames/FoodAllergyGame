using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartManager : Singleton<StartManager>{
	public GameObject sceneObjectParent;
	public GameObject SceneObjectParent{
		get{ return sceneObjectParent; }
	}

	public DinerEntranceUIController dinerEntranceUIController;
	public DinerEntranceUIController DinerEntranceUIController {
		get { return dinerEntranceUIController; }
	}

	public ShopEntranceUIController shopEntranceUIController;
	public ShopEntranceUIController ShopEntranceUIController {
		get { return shopEntranceUIController; }
	}

	public ChallengeMenuEntranceUIController challengeMenuEntranceUIController;
	public ChallengeMenuEntranceUIController ChallengeMenuEntranceUIController {
		get { return challengeMenuEntranceUIController; }
	}

	public RewardUIController rewardUIController;
	public RewardUIController RewardUIController{
		get{ return rewardUIController; }
	}

	public GameObject replayTutButton;
	public GameObject beaconNode;

	public AgeAskController ageAskController;
	private NotificationQueueDataAge ageNotification;

	public bool isShopAppearHideDinerOverride = false;

	void Start(){
		// Refresh tier calculation, always do this first
		TierManager.Instance.RecalculateTier();

		// First restaurant tutorial
		if(DataManager.Instance.GameData.Tutorial.IsTutorial1Done == false){
			shopEntranceUIController.Hide();
			DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "ChallengeTut1";
		}
		else {
			replayTutButton.SetActive(true);
			// Default case
			// TODO Refactor this logic
			// Show the deco entrance
		
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
			ShopEntranceUIController.ToggleClickable(false);
			DinerEntranceUIController.ToggleClickable(false);

			int oldTotalCash = CashManager.Instance.LastSeenTotalCash;
			int newTotalCash = CashManager.Instance.TotalCash;
			NotificationQueueDataTierProgress tierNotif = new NotificationQueueDataTierProgress(SceneUtils.START, oldTotalCash, newTotalCash);
			NotificationManager.Instance.AddNotification(tierNotif);
		}
		
		if(TierManager.Instance.IsNewUnlocksAvailable){
			if(!string.IsNullOrEmpty(DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge)){
				ShopEntranceUIController.ToggleClickable(false);
				DinerEntranceUIController.ToggleClickable(false);
			}
		if(TierManager.Instance.CurrentTier == 2 && !DataManager.Instance.GameData.DayTracker.HasCollectedAge) {
				//instantiate notification and then add it to queue when called it will show the panel
				ageNotification = new NotificationQueueDataAge();
				NotificationManager.Instance.AddNotification(ageNotification);
			}
			// Spawn the unlock drop pod here
			NotificationQueueDataReward rewardNotif = new NotificationQueueDataReward(SceneUtils.START);
			NotificationManager.Instance.AddNotification(rewardNotif);
        }

		// Load beacon for more crates
		if(TierManager.Instance.CurrentTier == 6 && !DataManager.Instance.GameData.DayTracker.IsMoreCrates) {
			GameObject beacon = Resources.Load("Beacon") as GameObject;
			GameObjectUtils.AddChild(beaconNode, beacon);
		}

		// Save game data again, lock down on an event
		DataManager.Instance.SaveGameData();
		GenerateCustomerList();
		GenerateUnlockedFoodStock();
	}

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
			if(cusData.Tier > TierManager.Instance.CurrentTier || cusData.Tier == -1) {
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
		List<ImmutableDataFood> unlockedFoodStock = DataLoaderFood.GetDataListWithinTier(TierManager.Instance.CurrentTier);

		// First remove all the foods that are not used for event
		ImmutableDataRemoveMenuSet currSet = DataLoaderRemoveMenuSet.GetData(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()).RemoveMenuSet);
		foreach(string foodID in currSet.RemoveMenuSet){
			if(foodID != "") {
				unlockedFoodStock.Remove(DataLoaderFood.GetData(foodID));
			}
		}
		unlockedFoodStock.Reverse();
		// Populate in FoodManager for use in MenuPlanning
		FoodManager.Instance.FoodStockList = unlockedFoodStock;
	}

	public void OnPlayButtonClicked(){
		// Check if special tutorial is set, load it as a challenge directly
		if(!string.IsNullOrEmpty(DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge)){
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT, showFoodTip: true);
		}
		else{
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.MENUPLANNING, "LoadingKeyMenu", "LoadingImageMenu");
        }
	}

	public void DecoButtonClicked(){
		DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance = false;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.DECO, "LoadingKeyDecoration");
	}

	public void ChallengeMenuButtonClicked(){
		DataManager.Instance.GameData.Challenge.IsFirstTimeChallengeEntrance = false;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.CHALLENGEMENU, "LoadingKeyDecoration");
	}
	
	public void CheatyScene() {
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.CHEATY);
	}

	public Vector3 GetEntrancePosition(StartMenuEntrances entrance) {
		switch(entrance) {
		case StartMenuEntrances.DinerEntrance:
			return DinerEntranceUIController.transform.position;
		case StartMenuEntrances.DecoEntrance:
			return ShopEntranceUIController.transform.position;
		case StartMenuEntrances.ChallengeEntrance:
			return ChallengeMenuEntranceUIController.transform.position;
		}
		Debug.LogError("Invalid entrance detected");
		return Vector3.zero;
	}

	public void OnLaunchTutorialButton() {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "ChallengeTut1";
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT, showFoodTip: true);
    }

	// Called from AgeAskController
	public void CollectAge(string age) {
		DataManager.Instance.GameData.DayTracker.HasCollectedAge = true;
		AnalyticsManager.Instance.SendAge(age);
		//send the finish call to wrap it up and move on
		ageNotification.Finish();
	}

	private  IEnumerator WaitOneFram() {
		yield return 0;
		ageAskController.ShowPanel();
	}
}
