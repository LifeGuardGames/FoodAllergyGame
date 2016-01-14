using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForFood : CustomerComponent {

	Customer self;

	public BehavWaitForFood(Customer cus) {
		self = cus;
		self.state = CustomerStates.WaitForFood;
		Act();
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
			BehavAllergyAttck aa = new BehavAllergyAttck(self);
			self.currBehav = aa;
			aa = null;
		}
		else {
			BehavEating eat = new BehavEating(self);
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
