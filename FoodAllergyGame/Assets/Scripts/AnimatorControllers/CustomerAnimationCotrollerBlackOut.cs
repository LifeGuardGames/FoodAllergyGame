using UnityEngine;
using System.Collections;

public class CustomerAnimationCotrollerBlackOut : CustomerAnimController{

	public Customer blackie;

	public void BlackOut() {
		skeletonAnim.state.SetAnimation(0, "AllHeartsLost", false).Complete +=
			delegate {
				RestaurantManager.Instance.Blackout();
				blackie.DestroySelf(0.0f);
			};
	}
	public void BlackOutButDontLeave() {
		skeletonAnim.state.SetAnimation(0, "AllHeartsLost", false).Complete +=
			delegate {
				RestaurantManager.Instance.Blackout();
				skeletonAnim.state.SetAnimation(0, "WaitingPassive", true);
            };
	}

	public override void SetRandomAllergyAttack() {
		if(isLimitAllergyAttackAnim) {
			skeletonAnim.state.SetAnimation(0, "AllergyAttack1", false);
		}
		else {
			int randomIndex = Random.Range(1, 3);   // Get random int between 1 and 2
			if(randomIndex == 2) {
				skeletonAnim.state.SetAnimation(0, "AllergyAttack" + randomIndex.ToString(), false);
			}
			else {
				puke.Play();
				skeletonAnim.state.SetAnimation(0, "AllergyAttack" + randomIndex.ToString(), false);
			}
		}
	}
	public override void SetSavedAllergyAttack() {
		puke.Stop();
		base.SetSavedAllergyAttack();
	}
}
