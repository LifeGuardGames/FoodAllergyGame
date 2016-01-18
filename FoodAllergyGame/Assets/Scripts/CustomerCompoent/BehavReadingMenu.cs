using UnityEngine;
using System.Collections;
using System;

public class BehavReadingMenu : CustomerComponent {


	public BehavReadingMenu() {
	}



	public override void Reason() {

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
		self.customerUI.ToggleWait(true);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
		

		
		
	}


}
