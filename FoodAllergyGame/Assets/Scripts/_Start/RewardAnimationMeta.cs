using UnityEngine;

/// <summary>
/// This is the metadata component of this, saves information about the actual notification
/// and passes it through when reward drop pod is clicked on
/// </summary>
public class RewardAnimationMeta : MonoBehaviour {
	public Animator dropPodAnimator;

	private NotificationQueueDataReward caller;

	// Save the information to pass through
	public void Init(NotificationQueueDataReward caller){
		this.caller = caller;
	}

	// Animation event
	public void ImpactAnimationEvent(){
		Camera.main.GetComponent<Animation>().Play();
	}

	void OnMouseUpAsButton(){
		StartManager.Instance.RewardUIController.Init(caller);
	}
}
