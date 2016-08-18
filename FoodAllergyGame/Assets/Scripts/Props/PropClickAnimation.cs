using UnityEngine;

/// <summary>
/// Attach this to a prop prefab with a collider and to make it clickable
/// Calls an animation or animator to do its tweening
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PropClickAnimation : MonoBehaviour {
	[Header("Optional Animations")]
	public Animation anim;
	public Animator animator;

	[Header("Optional Audio")]
	public string audioClipToPlay;
	public int audioVariations = 1;

	[Header("Optional Particle")]
	public ParticleSystem particle;

	public void OnMouseDown() {
		if(anim != null) {
			anim.Play();
		}
		if(animator != null) {
			animator.SetTrigger("Clicked");
		}
		if(string.IsNullOrEmpty(audioClipToPlay)) {
			AudioManager.Instance.PlayClip(audioClipToPlay, variations: audioVariations);
		}
		if(particle != null) {
			particle.Play();
        }
	}
}
