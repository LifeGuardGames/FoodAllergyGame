using UnityEngine;
using System.Collections;
using System;

public class BehavGossipWaitForFood : Behav {

	public BehavGossipWaitForFood() {

	}

	public override void Reason() {
		self.UpdateSatisfaction(1);
		self.customerAnim.SetEating();

		self.order = self.gameObject.transform.GetComponentInParent<Table>().FoodDelivered();
		self.order.GetComponent<BoxCollider>().enabled = false;
		self.order.GetComponent<Order>().ToggleShowOrderNumber(false);
		self.StopCoroutine("SatisfactionTimer");
		for(int i = 0; i < self.allergy.Count; i++) {
			if(self.order.GetComponent<Order>().allergy.Contains(self.allergy[i]) && !self.allergy.Contains(Allergies.None)) {
				self.state = CustomerStates.AllergyAttack;
				var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[7]);
				Behav aa = (Behav)Activator.CreateInstance(type);
				aa.self = self;
				aa.Act();
				self.currBehav = aa;
				aa = null;
				break;
			}
		}

		if(self.state == CustomerStates.WaitForFood) {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[4]);
			Behav eat = (Behav)Activator.CreateInstance(type);
			eat.self = self;
			eat.Act();
			self.currBehav = eat;
			eat = null;
			self.state = CustomerStates.Eating;
			self.StartCoroutine("EatingTimer");
			AudioManager.Instance.PlayClip("CustomerEating");
			Waiter.Instance.Finished();
		}
	}

	public override void Act() {
		self.state = CustomerStates.WaitForFood;
		int rand = UnityEngine.Random.Range(0, 10);
		if(rand > 7) {
			self.GetComponent<CustomerGossiper>().Gossip();
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
