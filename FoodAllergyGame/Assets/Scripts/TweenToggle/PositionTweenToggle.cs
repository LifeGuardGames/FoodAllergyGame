using UnityEngine;
using System.Collections;

/// <summary>
/// Position tween toggle.
/// Child of TweenToggle parent, Takes care of translation toggles
/// </summary>
public class PositionTweenToggle : TweenToggle {
	
	protected override void RememberPositions(){
		showingPos = gameObject.transform.localPosition;
		hiddenPos = gameObject.transform.localPosition + new Vector3(hideDeltaX, hideDeltaY, hideDeltaZ);
	}
	
	public override void Reset(){
		if (startsHidden){
			gameObject.transform.localPosition = hiddenPos;
			
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
			LeanTween.moveLocal(gameObject, showingPos, time)
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
			LeanTween.moveLocal(gameObject, hiddenPos, time)
				.setEase(easeHide)
					.setDelay(hideDelay)
						.setOnComplete(HideSendCallback);
		}
	}
}
