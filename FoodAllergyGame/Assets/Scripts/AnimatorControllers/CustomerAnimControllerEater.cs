using UnityEngine;
using System.Collections;

public class CustomerAnimControllerEater : CustomerAnimController {

	public override void SetSavedAllergyAttack() {
		Reset();
		Debug.LogWarning("anim Temp error PATCH - sean will fix this");
		skeleton.state.AddAnimation(0, "AllergySaved", false, 0f);
		skeleton.state.AddAnimation(0, "WaitingActive", true, 0f);
	}
}
