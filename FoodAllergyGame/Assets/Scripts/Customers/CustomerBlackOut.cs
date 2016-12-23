using UnityEngine;
using System.Collections;

public class CustomerBlackOut : Customer {

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.BlackOut;
		base.Init(num, mode);
	}

	public override void Init(int num, ImmutableDataEvents mode) {
		type = CustomerTypes.BlackOut;
		base.Init(num, mode);		
	}

	public override void UpdateSatisfaction(int delta) {
		base.UpdateSatisfaction(delta);
		if(delta < 0 && satisfaction != 0 && DataManager.Instance.GetChallenge() != "ChallengeTut2") {
			CustomerAnimationCotrollerBlackOut animBlackout = customerAnim as CustomerAnimationCotrollerBlackOut;
			animBlackout.BlackOutButDontLeave();
		}
	}
}
