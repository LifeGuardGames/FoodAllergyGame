using UnityEngine;
using System.Collections;
using System;

public class BehavVIPTutReadingMenu : CustomerComponent {

	public BehavVIPTutReadingMenu() {

	}

	public override void Reason() {
		throw new NotImplementedException();
	}

	public override void Act() {
		self.gameObject.GetComponent<CustomerVIPTut>().tutFingers.transform.GetChild(5).gameObject.SetActive(false);
		for(int i = 0; i < 4; i++) {
			RestaurantManager.Instance.GetTable(i).gameObject.GetComponent<BoxCollider>().enabled = true;
		}
	}
}
