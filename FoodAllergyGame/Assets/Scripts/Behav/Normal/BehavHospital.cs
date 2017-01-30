﻿/// <summary>
/// Customer leaving from going to the hospital due to allergy attack
/// </summary>
public class BehavHospital : Behav {
	public BehavHospital() {
	}

	public override void Reason() {
	}

	public override void Act() {
		self.isLeaveing = true;
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		RestaurantManager.Instance.GetTable(self.tableNum).inUse = false;
		DataManager.Instance.GameData.Tutorial.MissedMedic++;
		if(DataManager.Instance.GameData.Tutorial.MissedMedic >= 3) {
			DataManager.Instance.GameData.Tutorial.IsMedicTut2Done = false;
			DataManager.Instance.GameData.Tutorial.MissedMedic = 0;
		}
		if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {
			RestaurantManager.Instance.GetTable(self.tableNum).CustomerLeaving();
		}
		// Removes customer and bills the restaurant
		RestaurantManager.Instance.CustomerLeftFlatCharge(self, Medic.HospitalPrice, true);

		AudioManager.Instance.PlayClip("CustomerDead");
		self.DestroyOrder();
		self.DestroySelf(0);
	}
}
