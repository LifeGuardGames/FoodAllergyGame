﻿using UnityEngine;
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
	public TweenToggleDemux tweenDemux;

	public void Init(NotificationQueueDataNewItem caller, string decoID){
		originalCaller = caller;
		itemSprite.sprite = SpriteCacheManager.GetDecoSpriteDataByID(decoID);
		ShowPanel();
	}

	private void ShowPanel(){
		tweenDemux.Show();
		sunRaysRotate.Play();
	}

	public void OnShowFinished(){
		originalCaller.OnShowPanelFinished();
	}

	public void OnCloseButtonClicked(){
		tweenDemux.Hide();
	}

	public void OnHideFinished(){
		sunRaysRotate.Stop();
		originalCaller.Finish();
	}
}
