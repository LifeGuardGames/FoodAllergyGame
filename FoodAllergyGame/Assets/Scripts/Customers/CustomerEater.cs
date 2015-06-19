using UnityEngine;
using System.Collections;

public class CustomerEater : Customer {

	public override void NotifyLeave ()
	{
		if(state != CustomerStates.WaitForCheck && state != CustomerStates.InLine){
			Debug.Log(state);
			for (int i = 0; i < 4; i++){
				if(RestaurantManager.Instance.GetTable(i).GetComponent<Table>().seat.childCount > 0){
					if(RestaurantManager.Instance.GetTable(i).GetComponent<Table>().seat.GetChild(0).GetComponent<Customer>().state != CustomerStates.Invalid && RestaurantManager.Instance.GetTable(i).GetComponent<Table>().seat.GetChild(0).gameObject != this.gameObject){
						Destroy(RestaurantManager.Instance.GetTable(i).GetComponent<Table>().seat.GetChild(0).gameObject);
						RestaurantManager.Instance.GetTable(i).GetComponent<Table>().CustomerEaten();
						satisfaction++;
						customerUI.UpdateSatisfaction(satisfaction);
						break;
					}
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
