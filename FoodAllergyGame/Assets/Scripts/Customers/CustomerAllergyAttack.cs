using UnityEngine;
using System.Collections;

public class CustomerAllergyAttack : Customer {
	// this customer always has an allergy attack so we override eating to make it so
	public override void Eating(){
		satisfaction++;
		
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		customerAnim.SetEating(true);
		
		order = transform.GetComponentInParent<Table>().FoodDelivered();
		order.GetComponent<BoxCollider>().enabled = false;
		StopCoroutine("SatisfactionTimer");
		AllergyAttack();
	}
}
