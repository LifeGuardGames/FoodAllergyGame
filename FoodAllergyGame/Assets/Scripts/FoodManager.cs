using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains the menu and functions related to the menu and loading food
/// </summary>
public class FoodManager : Singleton<FoodManager>{

	// List for the user to choose from in MenuPlanning scene
	private List<ImmutableDataFood> foodStockList;
	public List<ImmutableDataFood> FoodStockList{
		set{ foodStockList = value; }
		get{ return foodStockList; }
	}

	// Compiled chosen list for use in restuarant
	private List<ImmutableDataFood> menuList;
	public List<ImmutableDataFood> MenuList{
		get{ return menuList; }
	}

	// Cost of building the menu, used for restaurant day over calculation
	private int menuCost = 0;
	public int MenuCost{
		get{ return menuCost; }
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
		bool allergyFood = false;
		bool allergenAdded = false;
		if(!RestaurantManager.Instance.isTutorial){
		
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
		}
		else{
			desiredFoodList.Add(DataLoaderFood.GetData("Food00"));
			desiredFoodList.Add(DataLoaderFood.GetData("Food03"));

		}
		return desiredFoodList;
	}

	#endregion
}
