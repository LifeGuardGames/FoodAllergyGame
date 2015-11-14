using UnityEngine;
using System.Collections;

public class CustomerTableSmasher : Customer {

	public override void Init (int num, ImmutableDataEvents mode) {
		base.Init (num, mode);
		type = CustomerTypes.TableSmasher;
	}

	//This customer will smash the table he is sitting at when he leaves unhappy
	//to do this we need to override the NotifyLeave function
	public override void NotifyLeave() {
		// check to make sure he isn't inline or waiting for the check as there is no table to smash while inline
		//and he needs to able to leave normally
		if(state != CustomerStates.WaitForCheck && state != CustomerStates.InLine){
			//flips the isBroken bool customers cannot be placed at tables where isBroken is true
			RestaurantManager.Instance.GetTable(tableNum).TableSmashed();

			// Downcast and play animation
			CustomerAnimControllerTableSmasher animTableSmasher = customerAnim as CustomerAnimControllerTableSmasher;
			animTableSmasher.SmashTable();

			//general customer leaving things
			RestaurantManager.Instance.CustomerLeft(customerID, satisfaction,1, transform.position, Time.time - spawnTime, false);
			Waiter.Instance.RemoveMeal(tableNum);
			KitchenManager.Instance.CancelOrder(tableNum);
			Destroy(this.gameObject, 6.5f);
		}
		else {
			//otherwise leave normally
			base.NotifyLeave();
		}
	}

	//void OnGUI() {
	//	if(GUI.Button(new Rect(100, 100, 100, 100), "SMASHHH")) {
	//		state = CustomerStates.WaitForFood;
	//		NotifyLeave();
	//	}
	//}
}

