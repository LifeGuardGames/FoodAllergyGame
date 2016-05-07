using UnityEngine;

public class BeaconUIController : MonoBehaviour {
	public Animator beaconAnimator;

	void Start() {
		beaconAnimator.Play("BeaconActive");
    }

	void OnMouseUpAsButton() {
		StartManager.Instance.ShopEntranceUIController.ToggleClickable(false);
		StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
		StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(false);
		StartManager.Instance.ShowParentalgate();
    }
}
