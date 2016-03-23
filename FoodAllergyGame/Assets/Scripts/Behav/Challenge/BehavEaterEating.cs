using UnityEngine;
using System.Collections;
using System;

public class BehavEaterEating : Behav {


	public BehavEaterEating() {

	}

	public override void Reason() {

		if(self.GetComponent<CustomerEater>().hasEaten) {
			if(RestaurantManager.Instance.GetTable(self.tableNum).cantLeave) {
				self.customerUI.ToggleStar(true);
			}
			self.attentionSpan = 10.0f * self.timer;
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
		else {
			self.GetComponent<CustomerEater>().hasEaten = true;
			self.DestroyOrder();
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[1]);
			Behav menu = (Behav)Activator.CreateInstance(type);
			menu.self = self;
			menu.Act();
			self.currBehav = menu;
			menu = null;
		}
	}

	public override void Act() {
		self.state = CustomerStates.Eating;

	}
}
