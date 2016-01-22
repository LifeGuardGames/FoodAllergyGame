using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForFood : Behav {


	public BehavWaitForFood() {
	}

	public override void Reason() {
		self.UpdateSatisfaction(1);
		self.customerAnim.SetEating();

		self.order = self.gameObject.transform.GetComponentInParent<Table>().FoodDelivered();
		self.order.GetComponent<BoxCollider>().enabled = false;
		self.order.GetComponent<Order>().ToggleShowOrderNumber(false);
		self.StopCoroutine("SatisfactionTimer");
		if(self.order.GetComponent<Order>().allergy.Contains(self.allergy) && self.allergy != Allergies.None) {
			self.state = CustomerStates.AllergyAttack;
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[6]);
			Behav aa = (Behav)Activator.CreateInstance(type);
			aa.self = self;
			aa.Act();
			self.currBehav = aa;
			aa = null;
		}
		else {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[3]);
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
	}
}
