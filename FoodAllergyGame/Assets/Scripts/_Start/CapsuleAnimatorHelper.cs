using UnityEngine;

public class CapsuleAnimatorHelper : MonoBehaviour {
	public RewardUIController rewardUIController;

	public void ShowItemInfo() {
		rewardUIController.ShowNextItemInfo();
	}
}
