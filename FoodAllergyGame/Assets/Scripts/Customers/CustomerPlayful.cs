using UnityEngine;
using System.Collections;

public class CustomerPlayful: Customer{

	public bool played = false;

	public override void Init(int num, ImmutableDataChallenge mode) {
		type = CustomerTypes.Playful;
		base.Init(num, mode);
	}

	public override void Init(int num, ImmutableDataEvents mode) {
		type = CustomerTypes.Impatient;
		base.Init(num, mode);
	}

	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		played = true;
		base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}
}
