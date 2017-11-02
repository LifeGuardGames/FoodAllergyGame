using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// NotificationManager is a class that handles a queue of Notifications
/// All time-dependent events should go through this NotificationManager into the queue
/// Notifications can range from getting a new item, or showing hud tier bar level up
/// </summary>
public class NotificationManager : Singleton<NotificationManager> {

	public event EventHandler<EventArgs> OnAllNotificationsFinished = null;

	public float delayBetweenNotifications = 0.5f;
	private Queue<NotificationQueueData> notificationQueue;
	public int NotificationQueueCount{
		get{ return notificationQueue.Count; }
	}

	private bool isNotificationActive = false;
	public bool IsNotificationActive{
		get{ return isNotificationActive; }
	}

	void Awake() {
		notificationQueue = new Queue<NotificationQueueData>();
	}

	void Start(){
		TryNextNotification();
	}

	public void AddNotification(NotificationQueueData notification){
		//AddToDatamanager(notification);
        notificationQueue.Enqueue(notification);
		TryNextNotification();
	}

	public void TryNextNotification(){
		if(!isNotificationActive && SceneManager.GetActiveScene().name == SceneUtils.START) {
			StartCoroutine(TryNextNotificationHelper());
		}
	}

	// NOTE: Only to be called from notification finish!
	public void TryNextNotificationInternal() {
		//Debug.Log("Being Called: " + SceneManager.GetActiveScene().name);
		StartCoroutine(TryNextNotificationHelper());
	}

	private IEnumerator TryNextNotificationHelper(){
		isNotificationActive = true;						// Lock the queue before frame skip, prevent asynchronous calls
		yield return 0;										// Wait one frame, make sure everything is nicely started/loaded
		
		if(notificationQueue.Count > 0){
			if(delayBetweenNotifications > 0) {
				yield return new WaitForSeconds(delayBetweenNotifications);
			}
			Debug.Log(notificationQueue.Count);
			Debug.Log(notificationQueue.Peek().ToString());
			NotificationQueueData pop = notificationQueue.Dequeue();
			/*if(pop.GetType().ToString() == "NotificationQueueDataReward") {
				notificationQueue.Clear();
			}*/
			pop.Start();
			//RemoveFromDatamanager(pop);
        }
		else{ 												// Everything completely done
			isNotificationActive = false;
			//Debug.Log("ALL notifications finished");
			if(SceneManager.GetActiveScene().name == SceneUtils.START) {
				StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
				StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);

				// Enable buttons
				StartManager.Instance.replayTutButton.SetActive(true);
				StartManager.Instance.showMissionsButton.SetActive(true);
				StartManager.Instance.musicButton.GetComponent<PositionTweenToggle>().Show();
				StartManager.Instance.soundButton.GetComponent<PositionTweenToggle>().Show();
				StartManager.Instance.musicButton.GetComponent<Toggle>().isOn = !AudioManager.Instance.isMusicOn;
				StartManager.Instance.soundButton.GetComponent<Toggle>().isOn = !AudioManager.Instance.isSoundEffectsOn;
				StartManager.Instance.musicButton.GetComponent<Toggle>().onValueChanged.AddListener((value) => AudioManager.Instance.ToggleMusic(!value));
				StartManager.Instance.soundButton.GetComponent<Toggle>().onValueChanged.AddListener((value) => AudioManager.Instance.ToggleSound(!value));

				if(TierManager.Instance.CurrentTier == 1) {
					AnalyticsManager.Instance.NotificationFunnel();
				}
				if(TierManager.Instance.CurrentTier >= 1) {
					bool isFirstTimeShop = DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance;
					StartManager.Instance.isShopAppearHideDinerOverride = isFirstTimeShop;
					StartManager.Instance.ShopEntranceUIController.Show(isFirstTimeShop);
				}
				// Keep diner unclickable ONLY when first time deco entrance and challenge
				if(StartManager.Instance.isShopAppearHideDinerOverride) {
					StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
				}
				else {
					StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
				}
				// Check if you need to load beacon for more crates
				if(TierManager.Instance.CurrentTier == 6 && !DataManager.Instance.GameData.DayTracker.IsMoreCrates) {
					if(StartManager.Instance.beaconNode.transform.childCount == 0) {
						GameObject beacon = Resources.Load("Beacon") as GameObject;
						GameObjectUtils.AddChild(StartManager.Instance.beaconNode, beacon);
						HUDManager.Instance.ToggleBeaconLock(true);
					}
				}
			}

			if(OnAllNotificationsFinished != null) {        // Throw event
				OnAllNotificationsFinished(this, EventArgs.Empty);
			}
		}
	}

	// NOTE: Use for cheaty button ONLY!!!
	public void DebugClearNotification() {
		notificationQueue = new Queue<NotificationQueueData>();
		isNotificationActive = false;
	}

	private void AddToDatamanager(NotificationQueueData que) {
        switch(que.GetType().ToString()){
			case "NotificationQueueDataStarPieceReward":
				if(!DataManager.Instance.GameData.DayTracker.NotifQueue.Contains("Piece")){
					DataManager.Instance.GameData.DayTracker.NotifQueue.Add("Piece");
				}
				break;
			case "NotificationQueueDataStarCoreReward":
				if(!DataManager.Instance.GameData.DayTracker.NotifQueue.Contains("core")) {
					DataManager.Instance.GameData.DayTracker.NotifQueue.Add("core");
				}
				break;
			case "NotificationQueueDataAge":
				if(!DataManager.Instance.GameData.DayTracker.NotifQueue.Contains("age")) {
					DataManager.Instance.GameData.DayTracker.NotifQueue.Add("age");
				}
				break;
			case "NotificationQueueDataReward":
				if(!DataManager.Instance.GameData.DayTracker.NotifQueue.Contains("reward")) {
					DataManager.Instance.GameData.DayTracker.NotifQueue.Add("reward");
				}
				break;
		}
	}
	private void RemoveFromDatamanager(NotificationQueueData que) {
		switch(que.GetType().ToString()) {

			case "NotificationQueueDataStarCoreReward":
				DataManager.Instance.GameData.DayTracker.NotifQueue.Remove("core");
				break;
			case "NotificationQueueDataStarPieceReward":
				DataManager.Instance.GameData.DayTracker.NotifQueue.Remove("Piece");
				break;
			case "NotificationQueueDataAge":
				DataManager.Instance.GameData.DayTracker.NotifQueue.Remove("age");
				break;
			case "NotificationQueueDataReward":
				DataManager.Instance.GameData.DayTracker.NotifQueue.Remove("reward");
				break;
			
		}
	}

	private void RebuildQueue() {
		int count = DataManager.Instance.GameData.DayTracker.NotifQueue.Count;
		for (int i = 0; i < count; i ++) {
			switch(DataManager.Instance.GameData.DayTracker.NotifQueue[i]) {
               case "core":
					NotificationQueueDataStarCoreReward core = new NotificationQueueDataStarCoreReward(SceneUtils.START);
					AddNotification(core);
					break;
				case "Piece":
					NotificationQueueDataStarPieceReward piece = new NotificationQueueDataStarPieceReward(SceneUtils.START, TierManager.Instance.OldTier, TierManager.Instance.CurrentTier);
					AddNotification(piece);
					break;
				case "age":
					NotificationQueueDataAge age = new NotificationQueueDataAge();
					AddNotification(age);
					break;
				case "reward":
					NotificationQueueDataReward reward = new NotificationQueueDataReward(SceneUtils.START);
					AddNotification(reward);
					break;
				
			}
		}
	}

	public void SkipNotifcation() {
		notificationQueue.Clear();
		TryNextNotification();
	}
}
