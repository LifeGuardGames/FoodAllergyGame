using UnityEngine;

// Used to skip tutorial on first time flow
public class SkipTutUIController : MonoBehaviour {
	public TweenToggleDemux demux;

	public void ShowPanel() {
		demux.Show();
	}

	public void HidePanel() {
		demux.Hide();
	}
}
