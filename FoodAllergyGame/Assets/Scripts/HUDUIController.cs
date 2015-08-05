using UnityEngine;
using System.Collections;

public class HUDUIController : MonoBehaviour {

	public TweenToggleDemux hudDemux;

	public void ShowHUD(){
		hudDemux.Show();
	}

	public void HideHUD(){
		hudDemux.Hide();
	}
}
