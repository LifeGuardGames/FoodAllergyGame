using UnityEngine;
using System.Collections;
using System;

public class BehavTableSmasherWaitForCheck : Behav {

	public BehavTableSmasherWaitForCheck() {

	}

	public override void Reason() {
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
		Behav leave = (Behav)Activator.CreateInstance(type);
		leave.self = self;
		leave.Act();
		leave = null;
	}

	public override void Act() {
		self.state = CustomerStates.WaitForCheck;
	}
}
