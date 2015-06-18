using UnityEngine;
using System.Collections;

public class Impatient :Customer {

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		switch(mode.CustomerMod){
			case "0":
			timer = 0.5f;
			break;
			case "1":
			timer = 0.8f;
			break;
			case "2":
			timer = 0.3f;
			break;
		}

	}
}
