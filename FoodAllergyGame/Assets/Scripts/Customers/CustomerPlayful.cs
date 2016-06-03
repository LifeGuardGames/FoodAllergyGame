using UnityEngine;
using System.Collections;

public class CustomerPlayful: Customer{

	public bool played = false;

	public override void Init(int num, ImmutableDataChallenge mode) {
		base.Init(num, mode);
		type = CustomerTypes.Playful;
	}

	public override void Init(int num, ImmutableDataEvents mode) {
		base.Init(num, mode);
		type = CustomerTypes.Playful;
	}

	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		played = true;
		base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}
}
