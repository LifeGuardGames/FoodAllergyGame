using UnityEngine;
using System.Collections;
using System;

public class BehavGossipWaitForFood : Behav {

	public BehavGossipWaitForFood() {

	}

	public override void Reason() {
		self.UpdateSatisfaction(1);
		self.customerAnim.SetEating();

		self.Order = self.gameObject.transform.GetComponentInParent<Table>().FoodDelivered();
		self.Order.GetComponent<BoxCollider>().enabled = false;
		self.Order.GetComponent<Order>().ToggleShowOrderNumber(false);
		self.StopCoroutine("SatisfactionTimer");
		for(int i = 0; i < self.allergy.Count; i++) {
			if(self.Order.GetComponent<Order>().allergy.Contains(self.allergy[i]) && !self.allergy.Contains(Allergies.None)) {
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
			self.currBehav = eat;
			eat.Act();
			eat = null;
			self.state = CustomerStates.Eating;
			self.StartCoroutine("EatingTimer");
			AudioManager.Instance.PlayClip("CustomerEating");
			Waiter.Instance.Finished();
		}
	}

	public override void Act() {
		self.state = CustomerStates.WaitForFood;
		if(RestaurantManager.Instance.TableList[self.tableNum].GetComponent<Table>().tableType != Table.TableType.FlyThru) {
			int rand = UnityEngine.Random.Range(0, 10);
			if(rand > 6) {
				var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
				Behav goss = (Behav)Activator.CreateInstance(type);
				goss.self = self;
				Debug.Log("Pre gossip: " + self.currBehav.ToString());
				self.gameObject.GetComponent<CustomerGossiper>().pastBehav = self.currBehav;
				self.currBehav = goss;
				Debug.Log(self.currBehav.ToString());
				goss.Act();
				goss = null;
			}
		}
	}
}
