using UnityEngine;
using System.Collections;

public class NotificationQueueDataTierProgress : NotificationQueueData {
	private int oldTotalCash;
	private int newTotalCash;

	NotificationQueueDataTierProgress(string allowedScene, int oldTotalCash, int newTotalCash){
		this.allowedScene = allowedScene;

		this.oldTotalCash = oldTotalCash;
		this.newTotalCash = newTotalCash;
	}
	
	public override void Start(){
		// Check one last time just to make sure
		if(oldTotalCash != newTotalCash){

		}
	}

	public override void Finish(){
		StartManager.Instance.SyncLastSeenTotalCash();
		base.Finish();
	}
}
