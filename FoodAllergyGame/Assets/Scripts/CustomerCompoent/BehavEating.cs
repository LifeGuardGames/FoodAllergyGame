using UnityEngine;
using System.Collections;

public class BehavEating :CustomerComponent {

	Customer self;

	public BehavEating(Customer cus) {
		self = cus;
		Act();
	}

	public override void Reason() {
		self.customerUI.ToggleStar(true);
		self.attentionSpan = 10.0f * self.timer;
		self.state = CustomerStates.WaitForCheck;
		self.StartCoroutine("SatisfactionTimer");
		AudioManager.Instance.PlayClip("CustomerReadyForCheck");
		self.DestroyOrder();
		BehavWaitForCheck chk = new BehavWaitForCheck(self);
		self.currBehav = chk;
		chk = null;
	}

	public override void Act() {
		self.state = CustomerStates.Eating;
	
	}
}
