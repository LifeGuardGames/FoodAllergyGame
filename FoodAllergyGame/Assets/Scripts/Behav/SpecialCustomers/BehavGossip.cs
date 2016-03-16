using UnityEngine;
using System.Collections;
using System;

public class BehavGossip : Behav {

	public BehavGossip() {

	}

	public override void Reason() {
		
	}

	public override void Act() {
		int rand = UnityEngine.Random.Range(0, 4);
		//Debug.Log ("Goissping " + rand.ToString());
		if(!RestaurantManager.Instance.GetTable(rand).isGossiped && RestaurantManager.Instance.GetTable(rand).inUse && rand != self.tableNum) {
			self.transform.SetParent(RestaurantManager.Instance.GetTable(rand).Node.transform);
			self.transform.localPosition = Vector3.zero;
			RestaurantManager.Instance.GetTable(rand).isGossiped = true;
			CustomerAnimationControllerGossiper goss = self.customerAnim as CustomerAnimationControllerGossiper;
			goss.Gossip();
		}
	}
}
