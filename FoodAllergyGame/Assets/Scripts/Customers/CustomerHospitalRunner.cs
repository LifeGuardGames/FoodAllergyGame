using UnityEngine;
using System.Collections;

public class CustomerHospitalRunner : Customer {

	public override void AllergyAttack ()
	{
		satisfaction = -20;
		customerUI.UpdateSatisfaction(satisfaction);
		customerAnim.SetSatisfaction(satisfaction);
		
		if(order.gameObject != null){
			Destroy(order.gameObject);
		}
		NotifyLeave();
	}
}
