﻿using UnityEngine;

public class StarAnimHelper : MonoBehaviour {
	public StarsUIController starsUIController;

	public void ChangeNextStarSprite() {
		starsUIController.OnNewStarSpriteEvent();
	}

	public void FinishAnimation() {
		starsUIController.OnRewardFinish();
    }

	// NOTE: Animator overrides all instances of sprite change if it is changed somewhere
	public void CoreRewardBaseSprite() {
		starsUIController.OnCoreRewardStart();
	}
}
