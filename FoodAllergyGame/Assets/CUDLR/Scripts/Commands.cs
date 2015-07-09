using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Net;

public static class Commands {

	[CUDLR.Command("list active customers", "Lists customers info")]
	public static void ListCustomer(){
		List<GameObject> customers = new List<GameObject>(RestaurantManager.Instance.getCurrentCustomers());
		foreach (GameObject go in customers){
			CUDLR.Console.Log("Type " + go.GetComponent<Customer>().type);
			CUDLR.Console.Log("Sate " + go.GetComponent<Customer>().state.ToString());
			CUDLR.Console.Log("Satisfaction " + go.GetComponent<Customer>().satisfaction.ToString());
			CUDLR.Console.Log("Timer Multiplier " + go.GetComponent<Customer>().timer.ToString());
			CUDLR.Console.Log("Table Number" + go.GetComponent<Customer>().tableNum.ToString());
		}
	}
}
