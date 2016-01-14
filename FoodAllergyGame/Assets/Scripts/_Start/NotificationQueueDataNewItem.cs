﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NotificationQueueDataNewItem : NotificationQueueData {
	public string itemIDToShow;
	public GameObject giftInstance;

	public NotificationQueueDataNewItem(string allowedScene, string itemID){
		this.allowedScene = allowedScene;
		itemIDToShow = itemID;
	}

	public override void Start(){
		if(allowedScene == SceneManager.GetActiveScene().name){

			GameObject giftPrefab = Resources.Load("GiftDropPod") as GameObject;
			giftInstance = GameObjectUtils.AddChildWithPositionAndScale(StartManager.Instance.SceneObjectParent, giftPrefab);

			// Pass the finish call here
			giftInstance.GetComponent<AnimationNewItem>().Init(this, itemIDToShow);
		}
		else{
			Finish();
		}
	}

	// Disable the drop pad when the show is finished
	public void DestroyGift(){
		GameObject.Destroy(giftInstance);
	}

	public override void Finish(){
		base.Finish();
	}
}
