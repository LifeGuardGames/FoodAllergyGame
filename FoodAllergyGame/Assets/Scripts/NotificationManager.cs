﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			NotificationQueueData pop = notificationQueue.Dequeue();
			/*if(pop.GetType().ToString() == "NotificationQueueDataReward") {
				notificationQueue.Clear();
			}*/
			pop.Start();
			//RemoveFromDatamanager(pop);
        }
		else{ 												// Everything completely done
			isNotificationActive = false;
			Debug.Log("ALL notifications finished");
			if(SceneManager.GetActiveScene().name == SceneUtils.START) {
				StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
				StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
				StartManager.Instance.challengeMenuEntranceUIController.ToggleClickable(true);
				StartManager.Instance.replayTutButton.SetActive(true);
				if(TierManager.Instance.CurrentTier == 1) {
					AnalyticsManager.Instance.NotificationFunnel();
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
						Debug.Log("LOADING BEACON");
						GameObject beacon = Resources.Load("Beacon") as GameObject;
						GameObjectUtils.AddChild(StartManager.Instance.beaconNode, beacon);
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
				if(!DataManager.Instance.GameData.DayTracker.notifQueue.Contains("Piece")){
					DataManager.Instance.GameData.DayTracker.notifQueue.Add("Piece");
				}
				break;
			case "NotificationQueueDataStarCoreReward":
				if(!DataManager.Instance.GameData.DayTracker.notifQueue.Contains("core")) {
					DataManager.Instance.GameData.DayTracker.notifQueue.Add("core");
				}
				break;
			case "NotificationQueueDataAge":
				if(!DataManager.Instance.GameData.DayTracker.notifQueue.Contains("age")) {
					DataManager.Instance.GameData.DayTracker.notifQueue.Add("age");
				}
				break;
			case "NotificationQueueDataReward":
				if(!DataManager.Instance.GameData.DayTracker.notifQueue.Contains("reward")) {
					DataManager.Instance.GameData.DayTracker.notifQueue.Add("reward");
				}
				break;
		}
	}
	private void RemoveFromDatamanager(NotificationQueueData que) {
		switch(que.GetType().ToString()) {

			case "NotificationQueueDataStarCoreReward":
				DataManager.Instance.GameData.DayTracker.notifQueue.Remove("core");
				break;
			case "NotificationQueueDataStarPieceReward":
				DataManager.Instance.GameData.DayTracker.notifQueue.Remove("Piece");
				break;
			case "NotificationQueueDataAge":
				DataManager.Instance.GameData.DayTracker.notifQueue.Remove("age");
				break;
			case "NotificationQueueDataReward":
				DataManager.Instance.GameData.DayTracker.notifQueue.Remove("reward");
				break;
			
		}
	}

	private void RebuildQueue() {
		int count = DataManager.Instance.GameData.DayTracker.notifQueue.Count;
		for (int i = 0; i < count; i ++) {
			Debug.Log(DataManager.Instance.GameData.DayTracker.notifQueue[i]);
			switch(DataManager.Instance.GameData.DayTracker.notifQueue[i]) {
               case "core":
					NotificationQueueDataStarCoreReward core = new NotificationQueueDataStarCoreReward(SceneUtils.START);
					Debug.Log("core");
					AddNotification(core);
					break;
				case "Piece":
					Debug.Log("Piece");
					NotificationQueueDataStarPieceReward piece = new NotificationQueueDataStarPieceReward(SceneUtils.START, TierManager.Instance.OldTier, TierManager.Instance.CurrentTier);
					AddNotification(piece);
					break;
				case "age":
					NotificationQueueDataAge age = new NotificationQueueDataAge();
					Debug.Log("age");
					AddNotification(age);
					break;
				case "reward":
					NotificationQueueDataReward reward = new NotificationQueueDataReward(SceneUtils.START);
					Debug.Log("reward");
					AddNotification(reward);
					break;
				
			}
		}
	}
	//void OnLevelWasLoaded(int level) {
	//	if(level == 2) {
			//RebuildQueue();
	//	}
//	}
}
