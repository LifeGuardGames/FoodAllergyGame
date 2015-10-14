using UnityEngine;
using System.Collections;

/// <summary>
/// This is the animation component of this, saves information about the actual notification
/// and passes it through when sprite is clicked on
/// </summary>
public class AnimationNewItem : MonoBehaviour {
	public SpriteRenderer podSprite;
	public Sprite crashSpriteResource;

	private NotificationQueueData caller;
	private string decoID;

	// Save the information to pass through
	public void Init(NotificationQueueData caller, string decoID){
		this.caller = caller;
		this.decoID = decoID;
	}

	// Animation event
	public void Impact(){
		Camera.main.GetComponent<Animation>().Play();
		podSprite.sprite = crashSpriteResource;
	}

	void OnMouseUpAsButton(){
		StartManager.Instance.NewItemUIController.Init(caller, decoID);
	}
}
