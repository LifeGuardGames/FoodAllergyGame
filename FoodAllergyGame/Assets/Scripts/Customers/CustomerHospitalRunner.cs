﻿using UnityEngine;
using System.Collections;

public class CustomerHospitalRunner : Customer{
	public override void Init(int num, ImmutableDataEvents mode){
		base.Init(num, mode);
		type = CustomerTypes.HospitalRunner;
	}

	// this customer will always auto goto the hospital if they have an allergy attack
	// simply override allergy attack and do the result of the allergy timer running out
	public override void AllergyAttack(){
		// -20 because the player should have been more careful
		SetSatisfaction(-20);

		//Also delete their food
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		NotifyLeave();
	}
}
