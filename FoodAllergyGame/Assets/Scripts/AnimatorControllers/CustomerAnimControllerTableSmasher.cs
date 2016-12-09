using UnityEngine;
using System.Collections;

public class CustomerAnimControllerTableSmasher : CustomerAnimController {
	public void SmashTable() {
		skeletonAnim.state.SetAnimation(0, "TableSmash", false).Complete +=
			delegate {
				skeletonAnim.state.SetAnimation(0, "WaitingPassive", true);
			};
		ParticleAndFloatyUtils.PlayTableSmashedParticle(transform.position);
	}

	public override void SetRandomAllergyAttack() {
		if(isLimitAllergyAttackAnim) {
			skeletonAnim.state.SetAnimation(0, "AllergyAttack1", false);
		}
		else {
			int randomIndex = Random.Range(1, 3);   // Get random int between 1 and 2
			if(randomIndex == 1) {
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
