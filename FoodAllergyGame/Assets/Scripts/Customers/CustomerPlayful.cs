using UnityEngine;
using System.Collections;

public class CustomerPlayful: Customer{

	public bool played = false;


	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		played = true;
		base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}
}
