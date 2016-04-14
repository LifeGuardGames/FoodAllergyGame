using UnityEngine;

public class ProductPageUIController : MonoBehaviour {
	public TweenToggleDemux demux;

	public void ShowPanel() {
		demux.Show();
	}

	public void HidePanel() {
		demux.Hide();
	}
}
