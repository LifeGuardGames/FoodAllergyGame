﻿using UnityEngine;
using System.Collections;

public class CustomerHospitalRunner : Customer {
	// this customer will always auto goto the hospital if they have an allergy attack
	// simply override allergy attack and do the result of the allergy timer running out
	public override void AllergyAttack ()
	{
		// -20 because the player should have been more careful
		satisfaction = -20;
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		//Also delete their food
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		NotifyLeave();
	}
}
