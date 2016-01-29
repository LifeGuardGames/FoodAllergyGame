using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForFoodReorder : Behav {

	public BehavWaitForFoodReorder() {

	}

	public override void Reason() {
		int rand = UnityEngine.Random.Range(0, 10);
		if(rand > 60) {
			self.Reorder();
		}
		else {
			self.customerUI.ToggleWait(true);
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[3]);
			Behav order = (Behav)Activator.CreateInstance(type);
			order.self = self;
			order.Act();
			self.currBehav = order;
			order = null;
		}
	}

	public override void Act() {
		self.state = CustomerStates.WaitForFood;
	}
}
