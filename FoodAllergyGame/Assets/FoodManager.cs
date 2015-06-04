using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodManager : Singleton<FoodManager>{

	//Contains the menu and functions related to the menu and loading food
	//SelectFoodItems chooses food items based off a supplied keyword
	//GenerateMenu creates a list of possible menu items from the Foodloader
	//FoodState gets the specific food data based off a food id
	public List <string> Menu;

	public void GenerateMenu(){
	
	}

	public void SelectFoodItems(string keyWord){
	
	}

	public ImmutableDataFood GetFood(string foodId){
	
	}

	// Use this for initialization
	void Start () {
		Menu = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
