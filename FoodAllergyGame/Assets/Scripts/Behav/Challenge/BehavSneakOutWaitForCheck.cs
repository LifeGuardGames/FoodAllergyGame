using UnityEngine;
using System.Collections;
using System;

public class BehavSneakOutWaitForCheck : Behav {

	public BehavSneakOutWaitForCheck() {

	}

	public override void Reason() {
		if(self.sneakOut) {
			RestaurantManager.Instance.CustomerLeftFlatCharge(self, 0, false);
			if(self.satisfaction > 3) {
				self.satisfaction = 3;
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
			if(self.hasPowerUp) {
				//Waiter.Instance.GivePowerUp();
			}
			AudioManager.Instance.PlayClip("CustomerLeave");
			self.DestroySelf(0);

		}
		else {
			self.StopCoroutine("SneakOut");
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[6]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
	}

	public override void Act() {
		self.state = CustomerStates.WaitForCheck;
		self.StartCoroutine("SneakOut");
	}
}
