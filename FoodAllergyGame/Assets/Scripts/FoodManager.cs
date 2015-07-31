using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains the menu and functions related to the menu and loading food
/// </summary>
public class FoodManager : Singleton<FoodManager>{

	// List for the user to choose from in MenuPlanning scene
	public List<ImmutableDataFood> foodStockList;
	public List<ImmutableDataFood> FoodStockList{
		get{ return foodStockList; }
	}

	// Compiled chosen list for use in restuarant
	public List<ImmutableDataFood> menuList;
	public List<ImmutableDataFood> MenuList{
		get{ return menuList; }
	}
		
	public List<string> tempMenu;

	private int menuCost;
	public int MenuCost{
		get{ return menuCost; }
	}

	void Awake(){
		menuCost = 0;
	}

	////////////////////////////////////

	#region MenuPlanning scene functions

	/// <summary>
	/// Creates a list of possible menu items from the FoodLoader
	/// </summary>
	public void GenerateMenu(List<string> _menuList, int cost){
		menuList = new List<ImmutableDataFood>();
		foreach(string foodID in _menuList){
			menuList.Add(DataLoaderFood.GetData(foodID));
		}
		menuCost = cost;
	}

	#endregion

	////////////////////////////////////

	#region Restaurant scene functions

	/// <summary>
	/// Chooses food items based off a supplied keyword from menuList
	/// </summary>
	public List<ImmutableDataFood> GetMenuFoodsFromKeyword(FoodKeywords keyword, Allergies _allergy){
		List<ImmutableDataFood> desiredFoodList = new List<ImmutableDataFood>();
	//	Debug.Log (menuList.Count);
		bool allergyFood = false;
		bool allergenAdded = false;
		while(desiredFoodList.Count < 2){
			allergyFood = false;
			int rand = Random.Range(0,menuList.Count);
			Debug.Log (menuList[rand].ID.ToString());
			if(!desiredFoodList.Contains(menuList[rand])){
				foreach(Allergies alg in menuList[rand].AllergyList){
					if(_allergy == alg){
						allergyFood = true;	
					}
				}
				if(allergyFood){
					if(!allergenAdded){
						allergenAdded = true;
						desiredFoodList.Add( menuList[rand]);
					}
				}
				else{
					desiredFoodList.Add(menuList[rand]);
				}
			}
		}
		return desiredFoodList;
	}

	#endregion
}
