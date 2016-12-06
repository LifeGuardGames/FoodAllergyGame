using UnityEngine;
using System.Collections;

public class CustomerHospitalRunner : Customer{
	public override void Init(int num, ImmutableDataEvents mode){
		type = CustomerTypes.HospitalRunner;
		base.Init(num, mode);
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.HospitalRunner;
		base.Init(num, mode);
	}
	// this customer will always auto goto the hospital if they have an allergy attack
	// simply override allergy attack and do the result of the allergy timer running out

}
