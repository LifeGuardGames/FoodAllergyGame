using UnityEngine;

public class BehavSaved : Behav {
	
	public BehavSaved() {

	}

	public override void Reason() {

	}

	public override void Act() {
		AudioManager.Instance.PlayClip("CustomerSaved");
		RestaurantManager.Instance.savedCustomers++;
		self.customerAnim.SetSavedAllergyAttack();
		RestaurantManager.Instance.sickCustomers.Remove(self.gameObject);
		self.UpdateSatisfaction(1);
		self.customerUI.ToggleAllergyAttack(false);
		self.state = CustomerStates.Saved;
		self.StopCoroutine("AllergyTimer");
		RestaurantManager.Instance.GetTable(self.tableNum).inUse = false;

		// Removes customer and bills the restaurant
		RestaurantManager.Instance.CustomerLeftFlatCharge(self, Medic.MedicPrice, true);

		AudioManager.Instance.PlayClip("CustomerLeave");
		self.DestroySelf(0);
	}
}
