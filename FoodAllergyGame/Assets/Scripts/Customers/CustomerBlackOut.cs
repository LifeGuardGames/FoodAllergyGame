using UnityEngine;
using System.Collections;

public class CustomerBlackOut : Customer {

	public override void NotifyLeave(){
		if(satisfaction == 0){
			RestaurantManager.Instance.Blackout();
		}
		base.NotifyLeave();
	}

}
