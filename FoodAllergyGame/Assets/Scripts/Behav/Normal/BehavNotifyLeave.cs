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
		
		if(RestaurantManager.Instance.actTables > 0) {
			
			//Debug.Log(self.state);
			if(self.satisfaction > 3) {
				self.satisfaction = 3;
			}
			if(self.Order != null) {
				self.Order.GetComponent<Order>().Canceled();
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
			if(self.state != CustomerStates.InLine &&  !self.isAnnoyed) {
				if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {
					self.completedMission = true;
					RestaurantManager.Instance.CustomerLeftSatisfaction(self, true, VIPMultiplier: RestaurantManager.Instance.GetTable(self.tableNum).VIPMultiplier);
				}
				else {
					self.completedMission = true;
					RestaurantManager.Instance.CustomerLeftSatisfaction(self, true);
				}
			}
			else {
				RestaurantManager.Instance.CustomerLeftFlatCharge(self, 0, true);
			}
			if(self.hasPowerUp) {
				//Waiter.Instance.GivePowerUp();
			}
			CheckMission();
			AudioManager.Instance.PlayClip("CustomerLeave");
			self.DestroySelf(0);
		}
	}

	public void CheckMission() {
		if(self.completedMission == true && self.failedMission == false) {
			if(DataManager.Instance.GameData.Daily.GetMissionByKey(self.customerIDMissionKey)!= null) {
				DataManager.Instance.GameData.Daily.GetMissionByKey(self.customerIDMissionKey).progress++;
            }
		}
	}
}
