using UnityEngine;
using System.Collections;
using System;

public class BehavTableSmasherNotifyLeave : CustomerComponent {

	public BehavTableSmasherNotifyLeave() {

	}

	public override void Reason() {
		throw new NotImplementedException();
	}

	public override void Act() {
		// check to make sure he isn't inline or waiting for the check as there is no table to smash while inline
		// and he needs to able to leave normally
		if(self.state != CustomerStates.WaitForCheck && self.state != CustomerStates.InLine && self.state != CustomerStates.Eating) {
			//flips the isBroken bool customers cannot be placed at tables where isBroken is true
			RestaurantManager.Instance.GetTable(self.tableNum).TableSmashed();

			// Downcast and play animation
			CustomerAnimControllerTableSmasher animTableSmasher = self.customerAnim as CustomerAnimControllerTableSmasher;
			animTableSmasher.SmashTable();

			//general customer leaving things
			RestaurantManager.Instance.CustomerLeft(self, false, self.satisfaction, 1, self.transform.position, Time.time - self.spawnTime, false);
			Waiter.Instance.RemoveMeal(self.tableNum);
			KitchenManager.Instance.CancelOrder(self.tableNum);
			self.DestroySelf(6.5f);
		}
		else {
			//otherwise leave normally
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[9]);
			CustomerComponent leave = (CustomerComponent)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
	}
}
