using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Customer leaving from satisfaction change only, NOT from allergyHospital or allergySaved
/// </summary>
public class BehavNotifyLeave : Behav {
	public BehavNotifyLeave() {
	}

	public override void Reason() {
	}

	public override void Act() {
		RestaurantManager.Instance.CustomerLeftSatisfaction(self, true);

		if(self.state != CustomerStates.InLine && self.state != CustomerStates.Saved) {
			RestaurantManager.Instance.GetTable(self.tableNum).CustomerLeaving();
			if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.FlyThru) {
				RestaurantManager.Instance.GetFlyThruTable().FlyThruLeave();
			}
		}
		else if(self.state == CustomerStates.InLine) {
			// Turn off customer highlights throughout the restaurant if it left and is selected
			if(Waiter.Instance.CurrentLineCustomer == self.gameObject) {
				RestaurantManager.Instance.CustomerLineSelectHighlightOff();
			}
			self.gameObject.transform.SetParent(null);
			RestaurantManager.Instance.lineCount--;
			RestaurantManager.Instance.lineController.FillInLine();
		}
		AudioManager.Instance.PlayClip("CustomerLeave");
		self.DestroySelf(0);
	}
}
