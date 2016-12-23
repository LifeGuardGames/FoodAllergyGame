using UnityEngine;
using System.Collections;
using System;

public class CustomerEater : Customer {

	public bool hasEaten = false;
	public Behav pastBehav;

	public override void Init (int num, ImmutableDataEvents mode)
	{
		type = CustomerTypes.Eater;
		base.Init (num, mode);
	}

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.Eater;
		base.Init(num, mode);
	}

	public override void UpdateSatisfaction(int delta) {
		base.UpdateSatisfaction(delta);
		if(satisfaction > 0 && delta < 0) {
			NotifyLeave();
		}
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
