using UnityEngine;
using System.Collections;
using System;

public class BehavHospitalRunnerAllergyAttack : CustomerComponent {

	BehavHospitalRunnerAllergyAttack() {
	}

	public override void Reason() {

	}

	public override void Act() {
		// -20 because the player should have been more careful
		self.SetSatisfaction(-20);

		//Also delete their food
		if(self.order.gameObject != null) {
			self.DestroyOrder();
		}
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		Medic.Instance.BillRestaurant(-100);
		ParticleUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(self.tableNum).gameObject.transform.position, -100);
		DataManager.Instance.GameData.Tutorial.MissedMedic++;
		if(DataManager.Instance.GameData.Tutorial.MissedMedic >= 3) {
			DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = false;
			DataManager.Instance.GameData.Tutorial.MissedMedic = 0;
		}

		AudioManager.Instance.PlayClip("CustomerDead");
		self.DestroySelf(0);
	}
}
