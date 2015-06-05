using UnityEngine;
using System.Collections;

public class Order : MonoBehaviour {

	//string FoodID the id of the food
	public string FoodID;
	//int TableNum the table number that ordered the dish used to find which table this goes too.
	public int TableNum;
	//bool IsCooked has this order been cooked
	public bool isCooked;
}
