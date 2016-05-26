using UnityEngine;
using System.Collections;
using System;

public class BehavHospitalRunnerAllergyAttack : Behav {

	public BehavHospitalRunnerAllergyAttack() {
	}

	public override void Reason() {

	}

	public override void Act() {
		Waiter.Instance.Finished();
		if(self.Order.gameObject != null) {
			self.DestroyOrder();
		}

		// Removes customer and bills the restaurant
		
		if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {
			RestaurantManager.Instance.GetTable(self.tableNum).CustomerLeaving();
        }
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		DataManager.Instance.GameData.Tutorial.MissedMedic++;
		RestaurantManager.Instance.GetTable(self.tableNum).inUse = false;
		RestaurantManager.Instance.CustomerLeftFlatCharge(self, Medic.HospitalPrice, true);
		if(DataManager.Instance.GameData.Tutorial.MissedMedic >= 3) {
			DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = false;
			DataManager.Instance.GameData.Tutorial.MissedMedic = 0;
		}

		AudioManager.Instance.PlayClip("CustomerDead");
		self.DestroySelf(0);
	}
}
