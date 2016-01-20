﻿using UnityEngine;
using System.Collections;
using System;

public class BehavPlayAreaTutReadingMenu : CustomerComponent {

	BehavPlayAreaTutReadingMenu() {

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
		self.gameObject.GetComponent<CustomerPlayAreaTut>().tutFingers.transform.GetChild(7).gameObject.SetActive(false);
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
	}
}
