using UnityEngine;
using UnityEngine.UI;

public class BeaconUIController : MonoBehaviour {
	public Animator beaconAnimator;

	void Start() {
		beaconAnimator.Play("BeaconActive");
    }

	void OnMouseUpAsButton() {
        PurchasingManager.Instance.ShowProductPage();
    }
}
