using UnityEngine;
using System.Collections;
using System;

public class BehavEaterNotifyLeave : Behav {


	 public BehavEaterNotifyLeave() {

	}

	public override void Reason() {
	}

	public override void Act() {
		if(self.tableNum == 5) {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[9]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
		else {
			// check to make sure the customer isnt waiting for the check or waiting in line or else the line may get rather short
			if(self.state != CustomerStates.WaitForCheck && self.state != CustomerStates.InLine && self.satisfaction <=0 || self.isAnnoyed) {
				//Debug.Log(state);
				//check each table for a victi...meal to eat
				for(int i = 0; i < RestaurantManager.Instance.actTables; i++) {
					// check to see if the table is in use
					if(RestaurantManager.Instance.GetTable(i).Seat.childCount > 0) {
						Customer targetCustomer = RestaurantManager.Instance.GetTable(i).Seat.GetChild(0).GetComponent<Customer>();
						// check the customer to make sure they arn't ordering or aren't currently being eaten and of course make sure he isn't eating himself
						if(targetCustomer.state != CustomerStates.Saved
							&& targetCustomer.state != CustomerStates.Eaten
							&& targetCustomer.state != CustomerStates.WaitForOrder
							&& targetCustomer.gameObject != self.gameObject) {

							// otherwise enjoy the meal
							RestaurantManager.Instance.GetTable(i).CustomerEaten();
							targetCustomer.GetComponent<Customer>().DestroySelf(0);
							self.UpdateSatisfaction(1);
							break;
						}
					}
				}
				Debug.Log(self.GetComponent<CustomerEater>().pastBehav.ToString());
				self.currBehav = self.GetComponent<CustomerEater>().pastBehav;
			}
			else if(self.state == CustomerStates.InLine) {
				var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
				Behav leave = (Behav)Activator.CreateInstance(type);
				leave.self = self;
				leave.Act();
				leave = null;
			}
			//if we need to just leave then leave
			if(self.state == CustomerStates.WaitForCheck) {
				var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
				Behav leave = (Behav)Activator.CreateInstance(type);
				leave.self = self;
				leave.Act();
				leave = null;
			}
		}
	}

}
