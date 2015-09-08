using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public TweenToggleDemux doorDemux;

	public void OpenAndCloseDoor(){
		doorDemux.Show();
	}

	private void CloseDoor(){
		doorDemux.Hide();
	}
}
