using UnityEngine;
using System.Collections;

public class CustomerAllergyAttack : Customer {

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
