using UnityEngine;
using System.Collections;

public class Impatient :Customer {

	public override void Init (int num)
	{
		timer = 0.5f;
		base.Init (num);
	}
}
