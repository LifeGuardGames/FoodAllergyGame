using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Notification spawning new item controller
/// Finish callback needs to be connected
/// </summary>
public class NewItemUIController : MonoBehaviour {
	private NotificationQueueDataNewItem originalCaller;

	public Image itemSprite;
	public RotateAroundCenter sunRaysRotate;
	public Animator newItemUIAnimator;
	public AlphaTweenToggle fadeToggle;

	public void Init(NotificationQueueDataNewItem caller, string decoID){
		gameObject.SetActive(true);
		originalCaller = caller;
		itemSprite.sprite = SpriteCacheManager.GetDecoSpriteDataByID(decoID);
		newItemUIAnimator.Play("NewItemShow");
	}

	public void OnDoneButtonClicked(){
		newItemUIAnimator.SetBool("Finish", true);
	}

	public void AnimEventFadeIn(){
		fadeToggle.Show();
	}

	public void AnimEventFadeOut(){
		fadeToggle.Hide();
	}

	public void AnimEventDestroyGift(){
		originalCaller.DestroyGift();
	}

	public void AnimEventOnSequenceFinished(){
		sunRaysRotate.Stop();
		originalCaller.Finish();
	}
}
