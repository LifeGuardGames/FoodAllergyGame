using UnityEngine;
using System.Collections;
using System;

public class BehavPlayAreaTutReadingMenu : Behav {

	public BehavPlayAreaTutReadingMenu() {

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
		self.gameObject.GetComponent<CustomerPlayAreaTut>().tutFingers.transform.GetChild(7).gameObject.SetActive(false);
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
	}
}
