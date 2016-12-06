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
		// check to make sure he isn't inline as there is no table to smash while inline
		// and he needs to able to leave normally
		if(self.state != CustomerStates.InLine || self.isAnnoyed) {

			// General customer leaving things
			Waiter.Instance.RemoveMeal(self.tableNum);
			KitchenManager.Instance.CancelOrder(self.tableNum);
			if(self.Order != null) {
				self.Order.GetComponent<Order>().Canceled();
			}
			RestaurantManager.Instance.CustomerLeftSatisfaction(self, true);

			// Flips the isBroken bool customers cannot be placed at tables where isBroken is true
			RestaurantManager.Instance.GetTable(self.tableNum).TableSmashed();

			// Downcast and play animation
			CustomerAnimControllerTableSmasher animTableSmasher = self.customerAnim as CustomerAnimControllerTableSmasher;
			animTableSmasher.SmashTable();


			self.DestroySelf(6.5f);
		}
		else if (self.state == CustomerStates.InLine) {
			GameObject Line = GameObject.Find("Line");
			if(Line.GetComponent<LineController>().lineList.Count > 2) {
				if(Line.GetComponent<LineController>().lineList[0].GetChild(0) != self.gameObject && Line.GetComponent<LineController>().lineList[0].GetChild(0).GetComponent<Customer>().type != CustomerTypes.TableSmasher) {
					// Downcast and play animation
					CustomerAnimControllerTableSmasher animTableSmasher = self.customerAnim as CustomerAnimControllerTableSmasher;
					animTableSmasher.SmashTable();
					Line.GetComponent<LineController>().lineList[0].GetChild(0).GetComponent<Customer>().NotifyLeave();
				}
              }
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[0]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
		else {
			// Otherwise leave normally
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
	}
}
