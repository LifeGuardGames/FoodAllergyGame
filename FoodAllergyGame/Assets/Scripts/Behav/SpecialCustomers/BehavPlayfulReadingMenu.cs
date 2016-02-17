using UnityEngine;
using System.Collections;
using System;

public class BehavPlayfulReadingMenu : Behav {

	public BehavPlayfulReadingMenu() {

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
		if(!self.gameObject.GetComponent<CustomerPlayful>().played){
			self.UpdateSatisfaction(-1);
		}
		self.StartCoroutine("ReadMenu");
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		self.state = CustomerStates.ReadingMenu;
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
	}
}
