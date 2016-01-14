using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForOrder : CustomerComponent {

	Customer self;

	public BehavWaitForOrder(Customer cus) {
		self = cus;
		Act();
	}

	public override void Reason() {
		BehavWaitForFood fod = new BehavWaitForFood(self);
		self.currBehav = fod;
		fod = null;
	}

	public override void Act() {
		self.state = CustomerStates.WaitForOrder;
		self.customerAnim.SetWaitingForOrder();
	}
}
