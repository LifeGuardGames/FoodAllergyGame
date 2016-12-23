using UnityEngine;
using UnityEngine.UI;

public class DropPodAnimatorHelper : MonoBehaviour {
	public ParticleSystem slamParticle;
	public Button dropPodButton;
	public Animator capsuleAnimator;

	public void PlaySlamParticle() {
		if(slamParticle != null) {
			slamParticle.Play();
		}
	}

	public void SetButtonInteractive() {
		dropPodButton.interactable = true;
	}

	public void SpawnRewardCapsule() {
		capsuleAnimator.Play("CapsulePop");
	}

	public void TurnButtonOff() {
		dropPodButton.gameObject.SetActive(false);
	}
}
