using UnityEngine;
using System.Collections;

public class CustomerImpatient :Customer {
	//this customer just has shorter timers than all the rest so we just need to modify init a bit
	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		type = CustomerTypes.Impatient;
		// this switch statement is simply meant to override the timer variable that the base class sets
		timer = mode.CustomerTimerMod/1.5f;

	}
}
