using UnityEngine;
using System.Collections;

/// <summary>
/// This is a transforming script that puts an arrow between two transforms
/// and makes the sprite face the front transform.
/// </summary>
public class ArrowBetween : MonoBehaviour {
	public Transform frontTransform;
	public Transform backTransform;

	void Update () {
		// Set the position between two points
		Vector3 middlePosition = frontTransform.position + backTransform.position;
		middlePosition = middlePosition / 2;
		transform.position = middlePosition;

		// Set the rotation
		float angleRotation = Mathf.Atan2(frontTransform.position.y - backTransform.position.y,
			frontTransform.position.x - backTransform.position.x) * Mathf.Rad2Deg;
		gameObject.transform.eulerAngles = new Vector3(0f, 0f, angleRotation);
	}
}
