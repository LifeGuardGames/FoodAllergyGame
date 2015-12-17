using UnityEngine;
using System.Collections;

public class AnimationStartRandomFrame : MonoBehaviour {

	public string clipName;

	void Start () {
		Animation anim = this.GetComponent<Animation>();
		anim[clipName].time = Random.Range(0.0f, 1.0f);
	}
}
