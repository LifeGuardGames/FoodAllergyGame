using UnityEngine;
using System.Collections;

public class CustomerCoolKid : Customer {

	public override void Playing(){
		satisfaction--;
		base.Playing();
	}
}
