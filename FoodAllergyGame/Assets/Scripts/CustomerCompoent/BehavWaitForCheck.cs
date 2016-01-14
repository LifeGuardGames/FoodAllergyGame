using UnityEngine;
using System.Collections;

public class BehavWaitForCheck : CustomerComponent {

	Customer self;

	public BehavWaitForCheck(Customer cus) {
		self = cus;
		Act();
	}

	public override void Reason() {
		BehavNotifyLeave leave = new BehavNotifyLeave(self);
		leave = null;
	}

	public override void Act() {
		self.state = CustomerStates.WaitForCheck;
	}
}
