using UnityEngine;
using System.Collections;

public class FirstStartAnimEvent : MonoBehaviour {
	
	public void FinishedAnim(){
		FirstStartManager.Instance.FinishedAnimation();
	}
}
