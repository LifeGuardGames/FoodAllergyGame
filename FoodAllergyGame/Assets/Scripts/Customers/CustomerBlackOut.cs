using UnityEngine;
using System.Collections;

public class CustomerBlackOut : Customer {

	public override void NotifyLeave(){
		base.NotifyLeave();
		BlackOut();
	}

	public void BlackOut(){
	}
}
