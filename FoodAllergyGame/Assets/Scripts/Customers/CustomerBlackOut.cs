using UnityEngine;
using System.Collections;

public class CustomerBlackOut : Customer {

	public override void Init(int num, ImmutableDataChallenge mode) {
		base.Init(num, mode);
		type = CustomerTypes.BlackOut;
	}

	public override void Init(int num, ImmutableDataEvents mode) {
		base.Init(num, mode);
		type = CustomerTypes.BlackOut;
	}

	public override void UpdateSatisfaction(int delta) {
		base.UpdateSatisfaction(delta);
		if(delta < 0 && satisfaction != 0) {
			CustomerAnimationCotrollerBlackOut animBlackout = customerAnim as CustomerAnimationCotrollerBlackOut;
			animBlackout.BlackOutButDontLeave();
		}
	}
}
