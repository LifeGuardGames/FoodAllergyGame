using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForCheck : CustomerComponent {


	public BehavWaitForCheck() {
		
	}

	public override void Reason() {
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[5]);
		CustomerComponent leave = (CustomerComponent)Activator.CreateInstance(type);
		leave.self = self;
		leave.Act();
		leave = null;
	}

	public override void Act() {
		self.state = CustomerStates.WaitForCheck;
	}
}
