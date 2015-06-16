using UnityEngine;
using System.Collections;

public class Eater : Customer {

	public override void NotifyLeave ()
	{
		if(state != CustomerStates.WaitForCheck || state != CustomerStates.InLine){
			for (int i = 0; i < 4; i++){
				if(RestaurantManager.Instance.GetTable(i).GetComponent<Table>().seat.childCount > 0){
					Destroy(RestaurantManager.Instance.GetTable(i).GetComponent<Table>().seat.GetChild(0).gameObject);
					RestaurantManager.Instance.GetTable(i).GetComponent<Table>().CustomerEaten();
					satisfaction++;
					break;
				}
			}
		}
		if(satisfaction == 0 || state == CustomerStates.WaitForCheck ||state == CustomerStates.InLine){
			ReallyLeave();
		}
	}

	public void ReallyLeave(){
		base.NotifyLeave();
	}
}
