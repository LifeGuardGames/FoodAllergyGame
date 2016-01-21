using UnityEngine;
using System.Collections;

/// <summary>
/// This is a transforming script that puts an arrow between two transforms
/// and makes the sprite face the front transform.
/// </summary>
public class ArrowBetween : MonoBehaviour {
	public Transform frontTransform;
	public Transform backTransform;
	public float percentageBetween = 0.5f;

	void Update () {
		// Set the position between two points
		transform.position = Vector3.Lerp(backTransform.position, frontTransform.position, percentageBetween);

		// Set the rotation
		float angleRotation = Mathf.Atan2(frontTransform.position.y - backTransform.position.y,
			frontTransform.position.x - backTransform.position.x) * Mathf.Rad2Deg;
		gameObject.transform.eulerAngles = new Vector3(0f, 0f, angleRotation);
	}
}
