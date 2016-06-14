using UnityEngine;

public class RestIntro : MonoBehaviour {
	public Animator animator;

	void Start() {
		animator.Play("Intro");
	}

	public void PlayIntroSound() {
		AudioManager.Instance.PlayClip("RestaurantDayStart");
	}
}
