using UnityEngine;
using System.Collections;
using System;

public class BehavTableSmasherSaved : Behav {

	public BehavTableSmasherSaved() {

	}

	public override void Reason() {
		throw new NotImplementedException();
	}

	public override void Act() {
		self.isLeaveing = true;
		self.StopCoroutine("AllergyTimer");
		if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {
			RestaurantManager.Instance.GetTable(self.tableNum).CustomerLeaving();
		}
		AudioManager.Instance.PlayClip("CustomerSaved");
		RestaurantManager.Instance.savedCustomers++;
		self.customerAnim.SetSavedAllergyAttack();
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		self.UpdateSatisfaction(1);
		self.customerUI.ToggleAllergyAttack(false);
		self.state = CustomerStates.Saved;

		RestaurantManager.Instance.GetTable(self.tableNum).inUse = false;


		// Removes customer and bills the restaurant
		RestaurantManager.Instance.CustomerLeftFlatCharge(self, Medic.MedicPrice, true);

		// Flips the isBroken bool customers cannot be placed at tables where isBroken is true
		RestaurantManager.Instance.GetTable(self.tableNum).TableSmashed();

		// Downcast and play animation
		CustomerAnimControllerTableSmasher animTableSmasher = self.customerAnim as CustomerAnimControllerTableSmasher;
		animTableSmasher.SmashTable();

		self.DestroySelf(6.5f);
	}
}
