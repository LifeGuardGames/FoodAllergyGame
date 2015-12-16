using UnityEngine;

public class PropTween : MonoBehaviour {

	public Vector3 finalPosition;
	public float tweenTime = 1;
	public LeanTweenType ease = LeanTweenType.linear;

	public GameObject propParentToDestroy;
	//public GameObject onCompleteObject;
	//public string onCompleteFunction;

	void Start() {
		LeanTween.moveLocal(gameObject, finalPosition, tweenTime).setEase(ease).setOnComplete(OnTweenComplete);
	}

	private void OnTweenComplete() {
		Destroy(propParentToDestroy);
		//if(onCompleteObject != null) {
		//	onCompleteObject.SendMessage(onCompleteFunction);
		//}
	}
}
