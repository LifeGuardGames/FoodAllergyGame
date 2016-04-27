using UnityEngine;
using System.Collections;
using System;

public class BehavGossipReadMenu : Behav {

	public BehavGossipReadMenu() {

	}

	public override void Reason() {
		self.customerUI.ToggleWait(true);
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[2]);
		Behav order = (Behav)Activator.CreateInstance(type);
		order.self = self;
		order.Act();
		self.currBehav = order;
		order = null;
	}

	public override void Act() {
		self.StartCoroutine("ReadMenu");
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
		self.state = CustomerStates.ReadingMenu;
		if(RestaurantManager.Instance.TableList[self.tableNum].GetComponent<Table>().tableType != Table.TableType.FlyThru) {
			int rand = UnityEngine.Random.Range(0, 10);
			if(rand > 0) {
				self.StopCoroutine("ReadMenu");
				var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[10]);
				Behav goss = (Behav)Activator.CreateInstance(type);
				goss.self = self;
				self.gameObject.GetComponent<CustomerGossiper>().pastBehav = self.currBehav;
				self.currBehav = goss;
				Debug.Log(self.currBehav.ToString());
				goss.Act();
				goss = null;
			}
		}
	}
}
