using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Notification used in a notification queue, abstract class so each one can be handled accordingly
/// Make sure each child calls the Start and Finish functions
/// NOTE: Notifications should not change any data, only read
/// </summary>
public abstract class NotificationQueueData {
	public string allowedScene;					// Scene where this notification is allowed

	public abstract void Start();

	// Call the notification manager to try to pop new
	public virtual void Finish(){
		NotificationManager.Instance.TryNextNotificationInternal();
	}
}
