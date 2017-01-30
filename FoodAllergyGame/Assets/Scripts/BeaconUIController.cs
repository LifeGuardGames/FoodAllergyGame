using UnityEngine;

public class BeaconUIController : MonoBehaviour {
	public Animator beaconAnimator;

	void Start() {
		beaconAnimator.Play("BeaconActive");
    }

	void OnMouseUpAsButton() {
		// TODO Uncomment this, feature removed from game
		//StartManager.Instance.ShowProductPage();
    }
}
