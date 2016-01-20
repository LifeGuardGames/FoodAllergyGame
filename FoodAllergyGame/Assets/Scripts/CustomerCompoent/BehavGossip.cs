using UnityEngine;
using System.Collections;
using System;

public class BehavGossip : CustomerComponent {

	public BehavGossip() {

	}

	public override void Reason() {
		self.transform.SetParent(RestaurantManager.Instance.GetTable(self.tableNum).Seat);
		self.transform.localPosition = Vector3.zero;
	}

	public override void Act() {
		int rand = UnityEngine.Random.Range(0, 4);
		//Debug.Log ("Goissping " + rand.ToString());
		if(!RestaurantManager.Instance.GetTable(rand).isGossiped && RestaurantManager.Instance.GetTable(rand).inUse && rand != self.tableNum) {
			self.transform.SetParent(RestaurantManager.Instance.GetTable(rand).Node.transform);
			self.transform.localPosition = Vector3.zero;
			RestaurantManager.Instance.GetTable(rand).isGossiped = true;
		}
	}
}
