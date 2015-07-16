using UnityEngine;
using System.Collections;

public class CustomerEater : Customer {

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		type = CustomerTypes.Eater;
	}

	// this customer will feed his hunger with other customers if his satisfation drops to 0
	// upon consuming a customer he will regain one satisfaction
	public override void NotifyLeave ()
	{
		// check to make sure the customer isnt waiting for the check or waiting in line or else the line may get rather short
		if(state != CustomerStates.WaitForCheck && state != CustomerStates.InLine ){
			//Debug.Log(state);
			//check each table for a victi...meal to eat
			for (int i = 0; i < RestaurantManager.Instance.actTables; i++){
				// check to see if the table is in use
				if(RestaurantManager.Instance.GetTable(i).Seat.childCount > 0){
					// check the customer to make sure they arn't ordering or arn't currently being eaten and of course make sure he isn't eating himself
					if(RestaurantManager.Instance.GetTable(i).Seat.GetChild(0).GetComponent<Customer>().state != CustomerStates.Invalid && RestaurantManager.Instance.GetTable(i).Seat.GetChild(0).GetComponent<Customer>().state != CustomerStates.WaitForOrder
					   && RestaurantManager.Instance.GetTable(i).Seat.GetChild(0).gameObject != this.gameObject){
						// otherwise enjoy the meal
						Destroy(RestaurantManager.Instance.GetTable(i).Seat.GetChild(0).gameObject);
						RestaurantManager.Instance.GetTable(i).CustomerEaten();
						satisfaction++;
						customerUI.UpdateSatisfaction(satisfaction);
						break;
					}
				}
			}
		}
		else if(state == CustomerStates.InLine){

		}
		//if we need to just leave then leave
		if(satisfaction == 0 || state == CustomerStates.WaitForCheck  || state == CustomerStates.AllergyAttack){
			ReallyLeave();
		}
	}

	public void ReallyLeave(){
		base.NotifyLeave();
	}
}
