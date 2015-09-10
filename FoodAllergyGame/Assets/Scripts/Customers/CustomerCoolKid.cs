using UnityEngine;
using System.Collections;

public class CustomerCoolKid : Customer {

	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		satisfaction--;
		base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}
}
