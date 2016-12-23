using UnityEngine;
using System.Collections;
using System;

public class BehavGossipEating : Behav {

	public BehavGossipEating() {

	}

	public override void Reason() {
		if(!RestaurantManager.Instance.GetTable(self.tableNum).cantLeave) {
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

	public override void Act() {
		self.state = CustomerStates.Eating;
		if(RestaurantManager.Instance.GetTable(self.tableNum).tableType != Table.TableType.FlyThru) { 
		int rand = UnityEngine.Random.Range(0, 10);
			if(rand > 6) {
				self.StopCoroutine("EatingTimer");
				var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
				Behav goss = (Behav)Activator.CreateInstance(type);
				goss.self = self;
				self.gameObject.GetComponent<CustomerGossiper>().pastBehav = self.currBehav;
				self.currBehav = goss;
				//Debug.Log(self.currBehav.ToString());
				goss.Act();
				goss = null;
			}
		}
	}
}
