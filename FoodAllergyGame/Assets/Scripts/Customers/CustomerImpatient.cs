using UnityEngine;
using System.Collections;

public class CustomerImpatient :Customer {
	//this customer just has shorter timers than all the rest so we just need to modify init a bit
	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		type = CustomerTypes.Impatient;
		// this switch statement is simply meant to override the timer variable that the base class sets
		switch(mode.CustomerTimerMod){
			case "0":
			timer = 0.5f;
			break;
			case "1":
			timer = 0.8f;
			break;
			case "2":
			timer = 0.3f;
			break;
		}

	}
}
