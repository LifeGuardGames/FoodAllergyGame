using UnityEngine;

public class BeaconUIController : MonoBehaviour {
	public Animator beaconAnimator;

	void Start() {
		beaconAnimator.Play("BeaconActive");
    }

	void OnMouseUpAsButton() {
        StartManager.Instance.ShowProductPage();
    }
}
