using UnityEngine;
using System.Collections;

public class CustomerAnimControllerEater : CustomerAnimController {

	public override void SetSavedAllergyAttack() {
		puke.Stop();
		Debug.LogWarning("anim Temp error PATCH - sean will fix this");
		skeletonAnim.state.SetAnimation(0, "AllergySaved", false);
		skeletonAnim.state.AddAnimation(0, "WaitingActive", true, 0f);
	}

	public void EatCustomer (){
		skeletonAnim.state.SetAnimation(0, "EatCustomerPass",false);
	}
	public void EatCustomerFail() {
		skeletonAnim.state.SetAnimation(0, "EatCustomerFail", false);
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
}
