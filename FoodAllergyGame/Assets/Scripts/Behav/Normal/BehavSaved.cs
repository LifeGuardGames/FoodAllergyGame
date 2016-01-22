using UnityEngine;
using System.Collections;
using System;

public class BehavSaved : Behav {


	public BehavSaved() {

	}

	public override void Reason() {

	}

	public override void Act() {
		Debug.Log("Saved");
		AudioManager.Instance.PlayClip("CustomerSaved");
		RestaurantManager.Instance.savedCustomers++;
		self.customerAnim.SetSavedAllergyAttack();
		Medic.Instance.BillRestaurant(-40);
		ParticleUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(self.tableNum).gameObject.transform.position, -40);
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		self.UpdateSatisfaction(1);
		self.customerUI.ToggleAllergyAttack(false);
		self.state = CustomerStates.Saved;
		self.StopCoroutine("AllergyTimer");
		RestaurantManager.Instance.GetTable(self.tableNum).inUse = false;
		RestaurantManager.Instance.CustomerLeft(self, false, self.satisfaction, 1, self.gameObject.transform.position, 720f, false);
		AudioManager.Instance.PlayClip("CustomerLeave");
		self.DestroySelf(0);
	}
}
