using UnityEngine;
using System.Collections;

public class CustomerPlayful: Customer{

	private bool played = false;

	public override void JumpToTable(int tableN){
		if(!played){
			satisfaction--;
		}
		base.JumpToTable(tableN);
	}

	public override void GoToPlayArea(Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		played = true;
		base.GoToPlayArea(playAreaSpot, spotIndex, deltaSatisfaction);
	}
}
