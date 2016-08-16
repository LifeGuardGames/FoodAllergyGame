using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this to a prop prefab with a collider and to make it clickable
/// Calls an animation or animator to do its tweening
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PropClickAnimation : MonoBehaviour {
	public Animation animation;
	public Animator animator;

	public void OnMouseDown() {
		if(animation != null) {
			animation.Play();
		}
		if(animator != null) {
			animator.SetTrigger("Clicked");
		}
	}
}
