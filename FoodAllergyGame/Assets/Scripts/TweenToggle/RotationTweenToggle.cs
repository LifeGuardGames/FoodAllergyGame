//// Copyright (c) 2015 LifeGuard Games Inc.

using UnityEngine;
using System.Collections;

/// <summary>
/// Rotation tween toggle.
/// Child of TweenToggle parent, Takes care of rotation toggles
/// </summary>
public class RotationTweenToggle : TweenToggle {

	protected override void RememberPositions(){
		showingPos = gameObject.transform.localEulerAngles;
		hiddenPos = gameObject.transform.localEulerAngles + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
	}
	
	public override void Reset(){
		if (startsHidden){
			gameObject.transform.localEulerAngles = hiddenPos;

		 	// Need to call show first
			isShown = false;
			isMoving = false;
		}
		else{
			// Need to call hide first
			isShown = true;
			isMoving = false;
		}
	}
	
	public override void Show(float time){
		if(!isShown){
			isShown = true;
			isMoving = true;

			LeanTween.cancel(gameObject);
			LeanTween.rotateLocal(gameObject, showingPos, time)
				.setEase(easeShow)
					.setDelay(showDelay)
						.setOnComplete(ShowSendCallback);
		}
	}

	public override void Hide(float time){
		if(isShown){
			isShown = false;
			isMoving = true;
            
			LeanTween.cancel(gameObject);
			LeanTween.rotateLocal(gameObject, hiddenPos, time)
				.setEase(easeHide)
					.setDelay(hideDelay)
						.setOnComplete(HideSendCallback);
		}
	}
}
