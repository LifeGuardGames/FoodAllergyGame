using UnityEngine;
using System.Collections;
using System;

public class BehavWaitForFood : Behav {


	public BehavWaitForFood() {
	}

	public override void Reason() {
		self.UpdateSatisfaction(1);
		self.customerAnim.SetEating();

		self.Order = self.gameObject.transform.GetComponentInParent<Table>().FoodDelivered();
		self.Order.GetComponent<BoxCollider>().enabled = false;
		self.Order.GetComponent<Order>().ToggleShowOrderNumber(false);
		self.StopCoroutine("SatisfactionTimer");
		for(int i = 0; i < self.allergy.Count; i++) {
			if(self.Order.GetComponent<Order>().allergy[i] == Allergies.Dairy) {
				RestaurantManager.Instance.dairyServed++;
			}
			else if(self.Order.GetComponent<Order>().allergy[i] == Allergies.Wheat) {
				RestaurantManager.Instance.wheatServed++;
			}
			else if(self.Order.GetComponent<Order>().allergy[i] == Allergies.Peanut) {
				RestaurantManager.Instance.peanutServed++;
			}
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
				for(int i = 0; i < self.Order.GetComponent<Order>().allergy.Count; i++) {
					if(self.Order.GetComponent<Order>().allergy[i] == Allergies.Dairy) {
						RestaurantManager.Instance.dairyServed++;
					}
					else if(self.Order.GetComponent<Order>().allergy[i] == Allergies.Wheat) {
						RestaurantManager.Instance.wheatServed++;
					}
					else if(self.Order.GetComponent<Order>().allergy[i] == Allergies.Peanut) {
						RestaurantManager.Instance.peanutServed++;
					}
			}
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
		if(self.Order.GetComponent<Order>().allergy.Contains(self.allergy[0]) && !RestaurantManager.Instance.isTutorial && !DataManager.Instance.GameData.Tutorial.IsTrashCanTutDone && self.allergy[0] != Allergies.None) {
			RestaurantManager.Instance.trashCanTutorial.SetActive(true);
            string foodSpriteName = DataLoaderFood.GetData(self.Order.GetComponent<Order>().foodID).SpriteName;
			RestaurantManager.Instance.trashCanTutorial.GetComponent<SickTutorialController>().Show(self.allergy[0], foodSpriteName);
			self.StopCoroutine("SatisfactionTimer");
		}
	}
}
