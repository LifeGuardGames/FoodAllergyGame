using UnityEngine;
using System.Collections;
using System;

public class BehavReadingMenu : Behav {


	public BehavReadingMenu() {
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
		self.customerAnim.SetReadingMenu();
		self.StartCoroutine("ReadMenu");
		self.state = CustomerStates.ReadingMenu;
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = (16.0f * self.timer) + 4.0f;
		self.StartCoroutine("SatisfactionTimer");
	}
}
