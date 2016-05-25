using UnityEngine;
using System.Collections;
using System;

public class CustomerEater : Customer {

	public bool hasEaten = false;
	public Behav pastBehav;

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		type = CustomerTypes.Eater;
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		base.Init(num, mode);
		type = CustomerTypes.Eater;
	}

	IEnumerator Leaving() {
		yield return new WaitForSeconds(6.0f);
		var type = Type.GetType(DataLoaderBehav.GetData(behavFlow).Behav[10]);
		Behav leave = (Behav)Activator.CreateInstance(type);
		leave.self = this;
		leave.Act();
		leave = null;
	}
	// this customer will feed his hunger with other customers if his satisfation drops to 0
	// upon consuming a customer he will regain one satisfaction
}
