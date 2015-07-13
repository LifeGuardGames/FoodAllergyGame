using UnityEngine;
using System.Collections;

public class CustomerGossiper : Customer{

	public override void JumpToTable (int tableN){
		base.JumpToTable (tableN);
		Debug.Log ("ReadingMenu");
		int rand = Random.Range(0,10);
		if(rand > 7){
			Gossip();
		}
	}

	public override void OrderTaken (ImmutableDataFood food){
		base.OrderTaken (food);
		Debug.Log ("orderTaken");
		int rand = Random.Range(0,10);
		if(rand > 7){
			Gossip();
		}
	}
	public override void Eating ()	{
		base.Eating ();
		Debug.Log ("Eating");
		int rand = Random.Range(0,10);
		if(rand > 7){
			Gossip();
		}
	}
	public void Gossip(){
		int rand = Random.Range(0,4);
		Debug.Log ("Goissping " + rand.ToString());
		if(!RestaurantManager.Instance.GetTable(rand).isGossiped && RestaurantManager.Instance.GetTable(rand).inUse && rand != tableNum){
			transform.SetParent(RestaurantManager.Instance.GetTable(rand).waiterSpot);
			transform.localPosition = Vector3.zero;
			RestaurantManager.Instance.GetTable(rand).isGossiped = true;
		}
	}

	public void GoAway(){
		transform.SetParent(RestaurantManager.Instance.GetTable(tableNum).Seat);
		transform.localPosition = Vector3.zero;
	}
}
