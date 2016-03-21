using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForOrder : Behav {


	public BehavWaitForOrder() {
	}

	public override void Reason() {
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[3]);
		Behav fod = (Behav)Activator.CreateInstance(type);
		fod.self = self;
		self.currBehav = fod;
		fod.Act();
		fod = null;
		
    }

	public override void Act() {
		self.state = CustomerStates.WaitForOrder;
		self.customerAnim.SetWaitingForOrder();
	}
}
