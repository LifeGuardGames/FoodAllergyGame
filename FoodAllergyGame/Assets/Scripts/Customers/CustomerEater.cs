using UnityEngine;
using System.Collections;

public class CustomerEater : Customer {

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		type = CustomerTypes.Eater;
	}

	// this customer will feed his hunger with other customers if his satisfation drops to 0
	// upon consuming a customer he will regain one satisfaction
}
