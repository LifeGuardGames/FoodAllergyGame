using UnityEngine;
using System.Collections;

public class CustomerPlayful: Customer {

	private bool played = false;

	public override void JumpToTable (int tableN)
	{
		if(!played){
			satisfaction--;
		}
		base.JumpToTable (tableN);
	}

	public override void Playing ()
	{
		played = true;
		base.Playing ();
	}
}
