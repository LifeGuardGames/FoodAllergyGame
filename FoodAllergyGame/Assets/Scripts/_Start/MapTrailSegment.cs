using UnityEngine;

public class MapTrailSegment : MonoBehaviour {
	public void Init(Transform startBase, Transform endBase) {
		transform.localPosition = startBase.localPosition;
		float length = Vector3.Distance(startBase.localPosition, endBase.localPosition);
		GetComponent<RectTransform>().sizeDelta = new Vector2(length, 2f);

		Vector3 direction = endBase.localPosition - startBase.localPosition;
		transform.localEulerAngles = new Vector3(0f, 0f,
			Vector3.Angle(Vector3.right, direction));

    }
}
