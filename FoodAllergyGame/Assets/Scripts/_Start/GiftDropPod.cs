using UnityEngine;
using System.Collections;

public class GiftDropPod : MonoBehaviour {
	public SpriteRenderer podSprite;
	public Sprite crashSpriteResource;

	// Animation event
	public void Impact(){
		Camera.main.GetComponent<Animation>().Play();
		podSprite.sprite = crashSpriteResource;
	}
}
