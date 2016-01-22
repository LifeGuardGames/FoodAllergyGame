using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForOrder : Behav {


	public BehavWaitForOrder() {
	}

	public override void Reason() {
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[2]);
		Behav fod = (Behav)Activator.CreateInstance(type);
		fod.self = self;
		fod.Act();
		self.currBehav = fod;
		fod = null;
		
    }

	public override void Act() {
		self.state = CustomerStates.WaitForOrder;
		self.customerAnim.SetWaitingForOrder();
	}
}
