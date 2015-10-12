using UnityEngine;
using System.Collections;

public class ButtonHelper : MonoBehaviour {
	public string buttonSoundDown = "Button1Down";
	public string buttonSoundUp = "Button1Up";

	public void PlaySoundDown(){
		AudioManager.Instance.PlayClip(buttonSoundDown);
	}

	public void PlaySoundUp(){
		AudioManager.Instance.PlayClip(buttonSoundUp);
	}
}
