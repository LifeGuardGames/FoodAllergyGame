using UnityEngine;

public class StarAnimHelper : MonoBehaviour {
	public StarsUIController starsUIController;

	public void ChangeNextStarSprite() {
		starsUIController.OnNewStarSpriteEvent();
	}

	public void FinishAnimation() {
		starsUIController.OnRewardFinish();
    }
}
