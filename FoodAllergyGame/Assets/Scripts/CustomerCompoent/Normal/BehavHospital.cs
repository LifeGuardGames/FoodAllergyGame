using UnityEngine;
using System.Collections;
using System;

public class BehavHospital : CustomerComponent {


	public BehavHospital() {
	}

	public override void Reason() {

	}

	public override void Act() {
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		Medic.Instance.BillRestaurant(-100);
		ParticleUtils.PlayMoneyFloaty(RestaurantManager.Instance.GetTable(self.tableNum).gameObject.transform.position, -100);
		DataManager.Instance.GameData.Tutorial.MissedMedic++;
		if(DataManager.Instance.GameData.Tutorial.MissedMedic >= 3) {
			DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = false;
			DataManager.Instance.GameData.Tutorial.MissedMedic = 0;
		}

		AudioManager.Instance.PlayClip("CustomerDead");
		
		self.SetSatisfaction(0);
		self.DestroyOrder();
		self.DestroySelf(0);

	}
}
