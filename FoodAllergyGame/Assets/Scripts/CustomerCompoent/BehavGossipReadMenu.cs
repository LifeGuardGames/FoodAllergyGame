using UnityEngine;
using System.Collections;
using System;

public class BehavGossipReadMenu : CustomerComponent {

	public BehavGossipReadMenu() {

	}

	public override void Reason() {
		self.customerUI.ToggleWait(true);
		var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[1]);
		CustomerComponent order = (CustomerComponent)Activator.CreateInstance(type);
		order.self = self;
		order.Act();
		self.currBehav = order;
		order = null;
	}

	public override void Act() {
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
		int rand = UnityEngine.Random.Range(0, 10);
		if(rand > 7) {
			var type = Type.GetType(DataLoaderBehav.GetData(self.behavFlow).Behav[1]);
			CustomerComponent goss = (CustomerComponent)Activator.CreateInstance(type);
			goss.self = self;
			goss.Act();
			self.gameObject.GetComponent<CustomerGossiper>().pastBehav = self.currBehav;
			self.currBehav = goss;
			goss = null;
		}
	}
}
