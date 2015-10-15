using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public TweenToggleDemux doorDemux;

	public void OpenAndCloseDoor(){
		doorDemux.Hide();
	}

	private void CloseDoor(){
		doorDemux.Show();
	}
}
