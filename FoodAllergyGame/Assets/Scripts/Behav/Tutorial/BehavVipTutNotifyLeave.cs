using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Customer leaving from satisfaction change only, NOT from allergyHospital or allergySaved
/// </summary>
public class BehavVipTutNotifyLeave : Behav {
	public BehavVipTutNotifyLeave() {
	}

	public override void Reason() {
	}

	public override void Act() {
		self.isLeaveing = true;
		if(RestaurantManager.Instance.actTables > 0) {
			//Debug.Log(self.state);
			if(self.satisfaction > 3) {
				self.satisfaction = 3;
			}
			AnalyticsManager.Instance.TutorialFunnel("VIP area Tut Completed");
			DataManager.Instance.GameData.Tutorial.IsSpeDecoTutDone = true;

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
			if(RestaurantManager.Instance.GetTable(self.tableNum).tableType == Table.TableType.VIP) {
				RestaurantManager.Instance.CustomerLeftSatisfaction(self, true, VIPMultiplier: RestaurantManager.Instance.GetTable(self.tableNum).VIPMultiplier);
			}
			else {
				RestaurantManager.Instance.CustomerLeftSatisfaction(self, true);
			}
			if(self.hasPowerUp) {
				//Waiter.Instance.GivePowerUp();
			}
			AudioManager.Instance.PlayClip("CustomerLeave");
			self.DestroySelf(0);
		}
	}
}