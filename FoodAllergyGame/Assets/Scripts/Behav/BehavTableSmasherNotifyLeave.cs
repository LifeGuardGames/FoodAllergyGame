using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Table Smasher will call other scripts for hospital and saved states so dont account for them here
/// </summary>
public class BehavTableSmasherNotifyLeave : Behav {

	public BehavTableSmasherNotifyLeave() {
	}

	public override void Reason() {
	}

	public override void Act() {
		// check to make sure he isn't inline or waiting for the check as there is no table to smash while inline
		// and he needs to able to leave normally
		if(self.state != CustomerStates.WaitForCheck && self.state != CustomerStates.InLine && self.state != CustomerStates.Eating) {
			// Flips the isBroken bool customers cannot be placed at tables where isBroken is true
			RestaurantManager.Instance.GetTable(self.tableNum).TableSmashed();

			// Downcast and play animation
			CustomerAnimControllerTableSmasher animTableSmasher = self.customerAnim as CustomerAnimControllerTableSmasher;
			animTableSmasher.SmashTable();

			// General customer leaving things
			Waiter.Instance.RemoveMeal(self.tableNum);
			KitchenManager.Instance.CancelOrder(self.tableNum);

			RestaurantManager.Instance.CustomerLeftSatisfaction(self, true);

			self.DestroySelf(6.5f);
		}
		else {
			// Otherwise leave normally
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[9]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
	}
}
