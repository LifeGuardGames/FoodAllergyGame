using UnityEngine;
using System.Collections;

public class BehavReadingMenu : CustomerComponent {

	Customer self;

	public BehavReadingMenu(Customer cus) {
		self = cus;
		Act();
	}

	public override void Reason() {

		BehavWaitForOrder order = new BehavWaitForOrder(self);
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
