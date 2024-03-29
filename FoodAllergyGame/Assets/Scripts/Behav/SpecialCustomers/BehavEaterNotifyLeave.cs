﻿using UnityEngine;
using System.Collections;
using System;

public class BehavEaterNotifyLeave : Behav {


	 public BehavEaterNotifyLeave() {

	}

	public override void Reason() {
	}

	public override void Act() {
		self.failedMission = true;
		if(self.tableNum == 5) {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
			Behav leave = (Behav)Activator.CreateInstance(type);
			leave.self = self;
			leave.Act();
			leave = null;
		}
		else {
			// check to make sure the customer isnt waiting for the check or waiting in line or else the line may get rather short
			if(self.state != CustomerStates.WaitForCheck || self.isAnnoyed) {
				//Debug.Log(state);
				//check each table for a victi...meal to eat
				for(int i = 0; i < 4; i++) {
					// check to see if the table is in use
					if(RestaurantManager.Instance.GetTable(i) != null) {
						if(RestaurantManager.Instance.GetTable(i).Seat.childCount > 0) {
							Customer targetCustomer = RestaurantManager.Instance.GetTable(i).Seat.GetChild(0).GetComponent<Customer>();
							// check the customer to make sure they arn't ordering or aren't currently being eaten and of course make sure he isn't eating himself
							if(targetCustomer.state != CustomerStates.Saved
								&& targetCustomer.state != CustomerStates.Eaten
								&& targetCustomer.gameObject != self.gameObject) {
								self.SetSatisfaction(1);
								self.UpdateSatisfaction(1);
								// otherwise enjoy the meal
								RestaurantManager.Instance.GetTable(i).CustomerEaten(self.gameObject);
								//targetCustomer.GetComponent<Customer>().DestroySelf(0);
								CustomerAnimControllerEater eat = self.customerAnim as CustomerAnimControllerEater;
								eat.EatCustomer();
								if(self.Order == null && self.state != CustomerStates.InLine) {
									var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[2]);
									Behav leave = (Behav)Activator.CreateInstance(type);
									leave.self = self;
									//leave.Act();
									leave = null;
									self.currBehav = leave;
								}
								else if (self.state == CustomerStates.InLine) {
									self.StopCoroutine("SatisfactionTimer");
									var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[0]);
									Behav leave = (Behav)Activator.CreateInstance(type);
									leave.self = self;
									leave.Act();
									self.currBehav = leave;
									leave = null;
								}
								else {
									var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[3]);
									Behav leave = (Behav)Activator.CreateInstance(type);
									leave.self = self;
									//leave.Act();
									leave = null;
									self.currBehav = leave;
								}
								break;
							}
						}
						else if(self.satisfaction == 0) {
							if(self.Order != null) {
								self.Order.GetComponent<Order>().Canceled();
							}
							CustomerAnimControllerEater eat = self.customerAnim as CustomerAnimControllerEater;
							eat.EatCustomerFail();
							self.GetComponent<CustomerEater>().StartCoroutine("Leaving");
						}
					}

				}
			}
			else {
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
