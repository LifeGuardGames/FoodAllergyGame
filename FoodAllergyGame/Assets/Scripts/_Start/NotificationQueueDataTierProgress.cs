using UnityEngine;
using System.Collections;

/// <summary>
/// Notification logic should be independent of the actual logical unlocks that happens
/// The actual unlocks are handled by the start manager in a serial call
/// </summary>
public class NotificationQueueDataTierProgress : NotificationQueueData {
	private int oldTotalCash;
	private int newTotalCash;

	public NotificationQueueDataTierProgress(string allowedScene, int oldTotalCash, int newTotalCash){
		this.allowedScene = allowedScene;
		this.oldTotalCash = oldTotalCash;
		this.newTotalCash = newTotalCash;
	}
	
	public override void Start() {

		// Check one last time just to make sure
		if(oldTotalCash != newTotalCash) {
			 HUDAnimator.Instance.StartStarChunkTweenSpawning(this, oldTotalCash, newTotalCash);	// This spawns the whole tier up process
		}
		else{
			Debug.Log("Invalid state to animate tier progress");
		}
	}
}
