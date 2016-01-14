using UnityEngine;
using System.Collections;
using System;

public class BehavNotifyLeave : CustomerComponent {

	Customer self;

	public BehavNotifyLeave(Customer cus) {
		self = cus;
		Act();
	}

	public override void Reason() {

	}

	public override void Act() {
		if(self.satisfaction > 0) {
			RestaurantManager.Instance.CustomerLeft(self, true, self.satisfaction, self.priceMultiplier, self.transform.position, Time.time - self.spawnTime, true);
		}

		else { 
			RestaurantManager.Instance.CustomerLeft(self, false, self.satisfaction, 1, self.transform.position, 720f, false);
		}

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
		self.DestroySelf();
	}
}
