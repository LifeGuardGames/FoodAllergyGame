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

	public int dayCashNetFromMenu;
	public int DayCashNetFromMenu{
		get{ return dayCashNetFromMenu; }
	}

	void Awake(){
		dayCashNetFromMenu = 0;
	}

	////////////////////////////////////

	#region MenuPlanning scene functions

	/// <summary>
	/// Creates a list of possible menu items from the FoodLoader
	/// </summary>
	public void GenerateMenu(List<string> _menuList){
		menuList = new List<ImmutableDataFood>();
		foreach(string foodID in _menuList){
			menuList.Add(DataLoaderFood.GetData(foodID));
			dayCashNetFromMenu -= DataLoaderFood.GetData(foodID).Cost;
		}
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
		bool allergenAdded = false;
		foreach(ImmutableDataFood foodData in menuList){
		///	Debug.Log ("Middle Step");
			foreach(FoodKeywords foodKeyword in foodData.KeywordList){
				//Debug.Log(foodKeyword.ToString());
				if(foodKeyword == keyword){
					foreach ( Allergies allergen in foodData.AllergyList){
						if(allergen == _allergy){
							if(!allergenAdded){
								allergenAdded = true;
								//Debug.Log (foodData.ID.ToString());
								desiredFoodList.Add(foodData);
							}
						}
						else{
							desiredFoodList.Add(foodData);
						}
					}
				}
			}
		}
		return desiredFoodList;
	}

	#endregion
}
