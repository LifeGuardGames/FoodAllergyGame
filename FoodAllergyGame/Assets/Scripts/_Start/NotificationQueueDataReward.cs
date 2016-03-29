using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Template of a NotificationQueueData
/// Launches the drop pod which then spawns the RewardUIController sequence for all unlocks at that tier.
/// </summary>
public class NotificationQueueDataReward : NotificationQueueData {
	public GameObject giftInstance;

	public NotificationQueueDataReward(string allowedScene){
		this.allowedScene = allowedScene;
	}

	public override void Start(){
		if(allowedScene == SceneManager.GetActiveScene().name){
			GameObject giftPrefab = Resources.Load("RewardDropPod") as GameObject;
			giftInstance = GameObjectUtils.AddChildWithPositionAndScale(StartManager.Instance.SceneObjectParent, giftPrefab);

			// Pass the finish call here
			giftInstance.GetComponent<RewardAnimationMeta>().Init(this);
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
