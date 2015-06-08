﻿using UnityEngine;
using System.Collections;

public class Order : MonoBehaviour{
	// ID of the food
	public string foodID;

	// Table number that ordered the dish used to find which table this goes too.
	public int tableNumber;

	// Is this order cooked?
	public bool isCooked;

	// Initialize the order when it is first spawned
	public void Init(string foodID, int tableNumber){
		this.foodID = foodID;
		this.tableNumber = tableNumber;
		isCooked = false;
	}
}