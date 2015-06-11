using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains the menu and functions related to the menu and loading food
/// </summary>
public class FoodManager : Singleton<FoodManager>{

	public List<ImmutableDataFood> foodStockList;	// List for the user to choose from in MenuPlanning scene
	public List<ImmutableDataFood> menuList;		// Compiled chosen list for use in restuarant
	public List<string> tempMenu;

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

		//test code
		tempMenu = new List<string>();
		tempMenu.Add("Food00");
		tempMenu.Add("Food01");
		tempMenu.Add("Food02");
		tempMenu.Add("Food03");
		tempMenu.Add("Food04");
		tempMenu.Add("Food05");
		tempMenu.Add("Food06");
		tempMenu.Add("Food07");
		tempMenu.Add("Food08");
		tempMenu.Add("Food09");
		GenerateMenu(tempMenu);
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
		List<ImmutableDataFood> desiredFoodList = new List<ImmutableDataFood>();
	//	Debug.Log (menuList.Count);
		foreach(ImmutableDataFood foodData in menuList){
		///	Debug.Log ("Middle Step");
			foreach(FoodKeywords foodKeyword in foodData.KeywordList){
				//Debug.Log(foodKeyword.ToString());
				if(foodKeyword == keyword){
					//Debug.Log (foodData.ID.ToString());
					desiredFoodList.Add(foodData);
				}
			}
		}
		return desiredFoodList;
	}

	#endregion
}
