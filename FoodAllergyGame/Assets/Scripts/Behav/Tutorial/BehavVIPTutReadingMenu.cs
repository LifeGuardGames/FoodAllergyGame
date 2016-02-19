using UnityEngine;
using System.Collections;
using System;

public class BehavVIPTutReadingMenu : Behav {

	public BehavVIPTutReadingMenu() {

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
		self.gameObject.GetComponent<CustomerVIPTut>().tutFingers.transform.GetChild(5).gameObject.SetActive(false);
		for(int i = 0; i < 4; i++) {
			RestaurantManager.Instance.GetTable(i).gameObject.GetComponent<BoxCollider>().enabled = true;
		}
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
	}
}
