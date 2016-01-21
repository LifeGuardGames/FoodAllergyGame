using UnityEngine;
using System.Collections;
using System;

public class BehavBlackoutNotifyLeave : CustomerComponent {

	public BehavBlackoutNotifyLeave() {

	}

	public override void Reason() {
		throw new NotImplementedException();
	}

	public override void Act() {
		if(self.satisfaction == 0) {
			RestaurantManager.Instance.Blackout();
		}
	}
}
