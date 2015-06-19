using UnityEngine;
using System.Collections;

public class CustomerTableSmasher : Customer {

	public override void NotifyLeave ()
	{
		if(state != CustomerStates.WaitForCheck && state != CustomerStates.InLine){
			RestaurantManager.Instance.GetTable(tableNum).GetComponent<Table>().TableSmashed();
			RestaurantManager.Instance.CustomerLeft(customerID, satisfaction);
			Waiter.Instance.RemoveMeal(tableNum);
			GameObject.Find("Kitchen").GetComponent<KitchenManager>().CancelOrder(tableNum);
			Destroy(this.gameObject);
		}

	}
}

