using UnityEngine;
using System.Collections;
using System;

public class BehavHospitalRunnerAllergyAttack : Behav {

	public BehavHospitalRunnerAllergyAttack() {
	}

	public override void Reason() {

	}

	public override void Act() {
		// -20 because the player should have been more careful
		self.SetSatisfaction(-20);

		//Also delete their food
		if(self.Order.gameObject != null) {
			self.DestroyOrder();
		}

		// Removes customer and bills the restaurant
		RestaurantManager.Instance.CustomerLeftFlatCharge(self, Medic.HospitalPrice, true);
		if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {
			self.customerUI.enabled = false;
		}
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		DataManager.Instance.GameData.Tutorial.MissedMedic++;
		if(DataManager.Instance.GameData.Tutorial.MissedMedic >= 3) {
			DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = false;
			DataManager.Instance.GameData.Tutorial.MissedMedic = 0;
		}

		AudioManager.Instance.PlayClip("CustomerDead");
		self.DestroySelf(0);
	}
}
