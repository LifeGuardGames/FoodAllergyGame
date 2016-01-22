using UnityEngine;
using System.Collections;
using System;

public class BehavGossipEating : Behav {

	public BehavGossipEating() {

	}

	public override void Reason() {
		self.customerUI.ToggleStar(true);
		self.attentionSpan = 10.0f * self.timer;
		self.state = CustomerStates.WaitForCheck;
		self.StartCoroutine("SatisfactionTimer");
		AudioManager.Instance.PlayClip("CustomerReadyForCheck");
		self.DestroyOrder();
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[4]);
		Behav chk = (Behav)Activator.CreateInstance(type);
		chk.self = self;
		chk.Act();
		self.currBehav = chk;
		chk = null;
	}

	public override void Act() {
		self.state = CustomerStates.Eating;
		int rand = UnityEngine.Random.Range(0, 10);
		if(rand > 7) {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[9]);
			Behav goss = (Behav)Activator.CreateInstance(type);
			goss.self = self;
			goss.Act();
			self.gameObject.GetComponent<CustomerGossiper>().pastBehav = self.currBehav;
			self.currBehav = goss;
			goss = null;
		}

	}
}
