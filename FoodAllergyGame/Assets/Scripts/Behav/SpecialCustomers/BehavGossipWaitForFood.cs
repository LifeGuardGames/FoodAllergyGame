using UnityEngine;
using System.Collections;
using System;

public class BehavGossipWaitForFood : Behav {

	public BehavGossipWaitForFood() {

	}

	public override void Reason() {
		self.customerUI.ToggleWait(true);
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[3]);
		Behav order = (Behav)Activator.CreateInstance(type);
		order.self = self;
		order.Act();
		self.currBehav = order;
		order = null;
	}

	public override void Act() {
		self.state = CustomerStates.WaitForFood;
		int rand = UnityEngine.Random.Range(0, 10);
		if(rand > 7) {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
			Behav goss = (Behav)Activator.CreateInstance(type);
			goss.self = self;
			goss.Act();
			self.gameObject.GetComponent<CustomerGossiper>().pastBehav = self.currBehav;
			self.currBehav = goss;
			goss = null;
		}
	}
}
