using UnityEngine;
using System.Collections;

public class CustomerImpatient :Customer {
	//this customer just has shorter timers than all the rest so we just need to modify init a bit
	public override void Init (int num, ImmutableDataEvents mode)
	{
		type = CustomerTypes.Impatient;
		base.Init (num, mode);
		// this switch statement is simply meant to override the timer variable that the base class sets
		timer = mode.CustomerTimerMod/1.5f;
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.Impatient;
		base.Init(num, mode);
	}
	public override void UpdateSatisfaction(int delta) {
		base.UpdateSatisfaction(delta);
		timer *= 0.75f;
		failedMission = true;
	}
}