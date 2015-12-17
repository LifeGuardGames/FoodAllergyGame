using UnityEngine;
using System.Collections;

public class PropTweenRotate : MonoBehaviour {

	public float initEulerAngle;	// Init position can be different from the start position
	public float endEulerAngle;
	public float firstTweenTime = 1;
	public float loopTweenTime = 1;
	public LeanTweenType ease = LeanTweenType.linear;

	public bool isLoop = false;
	public GameObject propParentToDestroy;

	private float tweenTimeAux;
	private float eulerAngleDifference;

	void Start() {
		tweenTimeAux = firstTweenTime;
		eulerAngleDifference = endEulerAngle - transform.localEulerAngles.z;
		TweenAction();
	}

	private void TweenAction() {
		LeanTween.cancel(gameObject);
		LeanTween.rotateAroundLocal(gameObject, Vector3.forward, eulerAngleDifference, tweenTimeAux)
			.setEase(ease)
			.setOnComplete(OnTweenComplete);
	}

	private void OnTweenComplete() {
		if(isLoop) {
			StartCoroutine(RestartNextFrame());
		}
		else {
			Destroy(propParentToDestroy);
		}
	}

	private IEnumerator RestartNextFrame() {
		yield return 0;
		transform.localEulerAngles = new Vector3(0, 0, initEulerAngle);
		eulerAngleDifference = endEulerAngle - transform.localEulerAngles.z;
		tweenTimeAux = loopTweenTime;
        TweenAction();
	}
}
