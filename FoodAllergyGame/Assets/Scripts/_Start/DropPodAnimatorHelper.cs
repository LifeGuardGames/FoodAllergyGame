using UnityEngine;

public class DropPodAnimatorHelper : MonoBehaviour {
	public RewardUIController rewardUIController;

	public ParticleSystem slamParticle;
	public Animator capsuleAnimator;

	public void PlaySlamParticle() {
		slamParticle.Play();
	}

	public void SpawnRewardCapsule() {
		capsuleAnimator.Play("CapsulePop");
    }
}
