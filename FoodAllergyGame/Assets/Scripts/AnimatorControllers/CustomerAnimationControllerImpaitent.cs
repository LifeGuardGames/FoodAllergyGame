﻿using UnityEngine;

public class CustomerAnimationControllerImpaitent : CustomerAnimController {

	public override void SetRandomAllergyAttack() {
		if(isLimitAllergyAttackAnim) {
			skeletonAnim.state.SetAnimation(0, "AllergyAttack1", false);
		}
		else {
			int randomIndex = Random.Range(1, 3);   // Get random int between 1 and 2
			if(randomIndex == 1) {
				skeletonAnim.state.SetAnimation(0, "AllergyAttack" + randomIndex.ToString(), false);
				puke.Play();
			}
			else {
				skeletonAnim.state.SetAnimation(0, "AllergyAttack" + randomIndex.ToString(), false);
			}
		}
	}
	public override void SetSavedAllergyAttack() {
		puke.Stop();
		base.SetSavedAllergyAttack();
	}
}
