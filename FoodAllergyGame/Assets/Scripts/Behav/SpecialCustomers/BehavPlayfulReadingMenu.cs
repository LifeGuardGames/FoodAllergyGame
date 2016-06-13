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
		self.state = CustomerStates.ReadingMenu;
		if(!self.gameObject.GetComponent<CustomerPlayful>().played && RestaurantManager.Instance.GetTable(self.tableNum).tableType != Table.TableType.FlyThru) {
			self.UpdateSatisfaction(-1);
			self.attentionSpan /= 2;
		}
		self.customerAnim.SetReadingMenu();
		self.StartCoroutine("ReadMenu");
		//get food choices 
		self.choices = FoodManager.Instance.GetTwoMenuFoodChoices(self.desiredFood, self.allergy);
		
		//stop the satisfaction timer, change the timer and then restart it
		self.attentionSpan = 21.0f * self.timer;
	}
}
