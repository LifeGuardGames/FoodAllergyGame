using UnityEngine;
using System.Collections;

public class CustomerCoolKid : Customer {

	public override void GoToPlayArea(Vector3 playAreaSpot){
		satisfaction--;
		base.GoToPlayArea(playAreaSpot);
	}
}
