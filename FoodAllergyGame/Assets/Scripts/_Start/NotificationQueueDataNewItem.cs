using UnityEngine;
using System.Collections;

public class NotificationQueueDataNewItem : NotificationQueueData {
	public string itemIDToShow;

	public NotificationQueueDataNewItem(string allowedScene, string itemID){
		this.allowedScene = allowedScene;
		itemIDToShow = itemID;
	}

	public override void Start(){
		if(allowedScene == Application.loadedLevelName){
			GameObject giftPrefab = Resources.Load("GiftDropPod") as GameObject;
			GameObject giftInstance = GameObjectUtils.AddChildWithPositionAndScale(StartManager.Instance.SceneObjectParent, giftPrefab);

			// Prepare finished delegate

			giftInstance.GetComponent<NewItemsUIController>().Init(this, itemIDToShow);
		}
		else{
			Finish();
		}
	}
}
