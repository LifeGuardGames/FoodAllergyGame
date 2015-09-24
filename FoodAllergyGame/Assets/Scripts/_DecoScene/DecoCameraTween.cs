using UnityEngine;
using System.Collections;

// Tweens the camera to a different part of the screen depending on what category is active
public class DecoCameraTween : MonoBehaviour {

	public float tweenTime = 0.5f;

	public Vector3 resetPosition;
	public Vector3 tablePosition;
	public Vector3 floorPosition;
	public Vector3 kitchenPosition;
	public Vector3 bathroomPosition;
	public Vector3 vipPosition;
	public Vector3 flyThruPosition;
	public Vector3 microwavePosition;
	public Vector3 playAreaPosition;

	public void TweenCamera(DecoTypes type){
		switch(type){
		case DecoTypes.None:
			LeanTween.moveLocal(Camera.main.gameObject, resetPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.Table:
			LeanTween.moveLocal(Camera.main.gameObject, tablePosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.Floor:
			LeanTween.moveLocal(Camera.main.gameObject, floorPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.Kitchen:
			LeanTween.moveLocal(Camera.main.gameObject, kitchenPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.Bathroom:
			LeanTween.moveLocal(Camera.main.gameObject, bathroomPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.VIP:
			LeanTween.moveLocal(Camera.main.gameObject, vipPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.FlyThru:
			LeanTween.moveLocal(Camera.main.gameObject, flyThruPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.Microwave:
			LeanTween.moveLocal(Camera.main.gameObject, microwavePosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		case DecoTypes.PlayArea:
			LeanTween.moveLocal(Camera.main.gameObject, playAreaPosition, tweenTime).setEase(LeanTweenType.easeInOutQuad);
			break;
		default:
			Debug.LogError("Bad deco type call");
			break;
		}
	}
}
