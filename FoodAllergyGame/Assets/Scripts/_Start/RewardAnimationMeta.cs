using UnityEngine;

/// <summary>
/// This is the metadata component of this, saves information about the actual notification
/// and passes it through when reward drop pod is clicked on
/// </summary>
public class RewardAnimationMeta : MonoBehaviour {
	public Animator dropPodAnimator;
	public ParticleSystem smokeTrail;

	private NotificationQueueDataReward caller;

	// Save the information to pass through
	public void Init(NotificationQueueDataReward _caller){
		caller = _caller;
	}

	public void StartFlightEvent() {

	}

	// Animation event
	public void ImpactAnimationEvent() {
		Camera.main.GetComponent<Animation>().Play();
		smokeTrail.Play();
		AudioManager.Instance.PlayClip("SupplyDropCrash");
	}

	void OnMouseUpAsButton() {
		StartManager.Instance.RewardUIController.Init(caller);
		dropPodAnimator.Play("DropPodDestroySelf");
    }

	public void DestroySelf() {
		Destroy(gameObject, 0.5f);
	}
}
