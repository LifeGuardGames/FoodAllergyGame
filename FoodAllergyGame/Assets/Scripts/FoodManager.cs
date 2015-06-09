using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains the menu and functions related to the menu and loading food
/// </summary>
public class FoodManager : Singleton<FoodManager>{

	public List<ImmutableDataFood> foodStockList;	// List for the user to choose from in MenuPlanning scene
	public List<ImmutableDataFood> menuList;		// Compiled chosen list for use in restuarant

	private static bool isCreated;

	void Awake(){
		// Make object persistent
		if(isCreated){
			// If There is a duplicate in the scene. delete the object and jump Awake
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		isCreated = true;
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
		}
	}

	#endregion

	////////////////////////////////////

	#region Restaurant scene functions

	/// <summary>
	/// Chooses food items based off a supplied keyword from menuList
	/// </summary>
	public List<ImmutableDataFood> GetMenuFoodsFromKeyword(FoodKeywords keyword){
		List<ImmutableDataFood> selectedFoods = new List<ImmutableDataFood>();
		for (int i = 0; i < menuList.Count; i++){
			for (int f = 0; i < menuList[i].KeywordList.Count; f++){
				if(menuList[i].KeywordList[f] == keyword){
					selectedFoods.Add(menuList[i]);
				}
			}
		}
		return selectedFoods;
	}

	#endregion
}
