using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
		notificationQueue.Enqueue(notification);
		TryNextNotification();
	}

	public void TryNextNotification(){
		if(!isNotificationActive){
			StartCoroutine(TryNextNotificationHelper());
		}
	}

	// NOTE: Only to be called from notification finish!
	public void TryNextNotificationInternal() {
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
			pop.Start();
		}
		else{ 												// Everything completely done
			isNotificationActive = false;

			if(Application.loadedLevelName == SceneUtils.START) {
				StartManager.Instance.DecoEntranceUIController.ToggleClickable(false);
				StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
			}

			if(OnAllNotificationsFinished != null) {        // Throw event
				OnAllNotificationsFinished(this, EventArgs.Empty);
			}
		}
	}
}
