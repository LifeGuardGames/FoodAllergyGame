using UnityEngine;
using System.Collections;
using System;

public class BehavAllergyAttck : CustomerComponent {

	Customer self;

	public BehavAllergyAttck(Customer cus) {
		self = cus;
	}

	public override void Reason() {
		if(self.saved) {
			BehavSaved sav = new BehavSaved(self);
			self.currBehav = sav;
			sav = null;
		}
		else {
			BehavHospital hos = new BehavHospital(self);
			self.currBehav = hos;
			hos = null;
		}

	}

	public override void Act() {
		self.DestroyOrder();
		self.priceMultiplier = 1;
		self.customerUI.ToggleAllergyAttack(true);
		self.customerAnim.SetRandomAllergyAttack();
		RestaurantManager.Instance.sickCustomers.Add(self.gameObject);
		AudioManager.Instance.PlayClip("CustomerAllergyAttackAudio");

		// Show tutorial if needed
		if(DataManager.Instance.GameData.Tutorial.IsMedicTut2Done) {
			Waiter.Instance.Finished();
			self.StartCoroutine("AllergyTimer");
		}
		else {
			if(!DataManager.Instance.GameData.Tutorial.IsMedicTut1Done) {
				DataManager.Instance.GameData.Tutorial.IsMedicTut1Done = true;
			}
			else {
				DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = true;
			}
			Waiter.Instance.isMedicTut = true;
			Waiter.Instance.CancelMove();
			RestaurantManager.Instance.medicTutorial.SetActive(true);
			//TouchManager.Instance.PauseQueue();
			string foodSpriteName = DataLoaderFood.GetData(self.order.GetComponent<Order>().foodID).SpriteName;
			RestaurantManager.Instance.medicTutorial.GetComponent<SickTutorialController>().Show(self.allergy, foodSpriteName);
		}
	}
}
