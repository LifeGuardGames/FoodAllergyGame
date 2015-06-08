using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodManager : Singleton<FoodManager>{

	//Contains the menu and functions related to the menu and loading food
	//SelectFoodItems chooses food items based off a supplied keyword
	//GenerateMenu creates a list of possible menu items from the Foodloader
	//FoodState gets the specific food data based off a food id
	public List<ImmutableDataFood> menu;

	public void GenerateMenu(string foodId){
		menu.Add(DataLoaderFood.GetData(foodId));
	}

	public List<ImmutableDataFood> SelectFoodItems(FoodKeywords keyWord){
		List<ImmutableDataFood> selectedFood = new List<ImmutableDataFood>();
		for (int i = 0; i < menu.Count; i++){
			for (int f = 0; i < menu[i].KeywordList.Count; f++){
				if(menu[i].KeywordList[f] == keyWord){
					selectedFood.Add(menu[i]);
				}
			}
		}
		return selectedFood;
	}

	public ImmutableDataFood GetFood(string foodId){
		if(DataLoaderFood.GetData(foodId)!= null){
			return DataLoaderFood.GetData(foodId);
		}
		else{
			return null;// TODO
		}
	}

	// Use this for initialization
	void Start(){
		menu = new List<ImmutableDataFood>();
	}
}
