﻿using UnityEngine;
using System.Collections;

public class CustomerAnimControllerEater : CustomerAnimController {

	public override void SetSavedAllergyAttack() {
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
}
