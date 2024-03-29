﻿using UnityEngine;
using System.Collections;
using System;

public class BehavMakeYourMindWaitForOrder : Behav {

	public BehavMakeYourMindWaitForOrder() {

	}

	public override void Reason() {
		int rand = UnityEngine.Random.Range(0, 10);
		if(rand > 6) {
			self.Reorder();
		}
		else {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[3]);
			Behav fod = (Behav)Activator.CreateInstance(type);
			fod.self = self;
			fod.Act();
			self.currBehav = fod;
			fod = null;
		}
	}

	public override void Act() {
		self.state = CustomerStates.WaitForOrder;
		self.customerAnim.SetWaitingForOrder();
	}
}
