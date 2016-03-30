using UnityEngine;

public class DropPodAnimatorHelper : MonoBehaviour {
	public RewardUIController rewardUIController;
	
	public void OnDropPodOpenAnimationDone() {
		rewardUIController.OnDropPodOpenAnimationDone();
    }
}
