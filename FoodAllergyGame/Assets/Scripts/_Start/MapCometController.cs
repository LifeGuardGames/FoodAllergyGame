using UnityEngine;

public class MapCometController : MonoBehaviour {
	public Animator cometAnim;
	public ParticleSystem fireParticle;
	public ParticleSystem smashParticle;

	public void SmashComet() {
		cometAnim.SetTrigger("SmashComet");
	}

	// Animation event
	public void ToggleFireParticle(int isPlay) {
		if(isPlay == 1) {
			fireParticle.Play();
		}
		else {
			fireParticle.Stop();
		}
	}

	// Animation event
	public void PlaySmashParticle() {
		smashParticle.Play();
	}
}
