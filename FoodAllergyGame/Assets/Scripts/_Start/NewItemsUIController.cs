using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Notification spawning new item controller
/// Finish callback needs to be connected
/// </summary>
public class NewItemsUIController : MonoBehaviour {
	NotificationQueueData originalCaller;

	public Image itemSprite;
	public RotateAroundCenter sunRaysRotate;
	public TweenToggleDemux tweenDemux;

	public void Init(NotificationQueueData caller, string decoID){
		originalCaller = caller;
		itemSprite.sprite = SpriteCacheManager.GetDecoSpriteData(decoID);
		ShowPanel();
	}

	private void ShowPanel(){
		tweenDemux.Show();
		sunRaysRotate.Play();
	}

	public void OnCloseButtonClicked(){
		tweenDemux.Hide();
	}

	public void OnHideFinished(){
		sunRaysRotate.Stop();
	}

	public void Finish(){
		originalCaller.Finish();
	}
}
