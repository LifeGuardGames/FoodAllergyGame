using UnityEngine;
using System.Collections;

public class PropTween : MonoBehaviour {

	public Vector3 initPosition;
	public Vector3 finalPosition;
	public float tweenTime = 1;
	public LeanTweenType ease = LeanTweenType.linear;

	public bool isLoop = false;
	public GameObject propParentToDestroy;

	void Start() {
		TweenAction();
	}

	private void TweenAction() {
		LeanTween.moveLocal(gameObject, finalPosition, tweenTime).setEase(ease).setOnComplete(OnTweenComplete);
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
		transform.localPosition = initPosition;
        TweenAction();
	}
}
