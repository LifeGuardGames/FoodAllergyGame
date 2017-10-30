using System.Collections;
using UnityEngine;
using System;

public class BehavEatingToGo : Behav { 

	public BehavEatingToGo() {

	}

	public override void Reason() {

	}

	public override void Act() {
		if(!RestaurantManager.Instance.GetTable(self.tableNum).cantLeave) {
			self.customerUI.ToggleStar(true);
		}
		self.attentionSpan = self.eatTimer + (8.0f * self.timer);
		self.state = CustomerStates.WaitForCheck;
		self.StartCoroutine("SatisfactionTimer");
		AudioManager.Instance.PlayClip("CustomerReadyForCheck");
		self.DestroyOrder();
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[5]);
		Behav chk = (Behav)Activator.CreateInstance(type);
		chk.self = self;
		chk.Act();
		self.currBehav = chk;
		chk = null;

	}
}
