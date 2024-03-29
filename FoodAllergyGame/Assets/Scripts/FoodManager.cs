﻿using UnityEngine;
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

	public ImmutableDataFood specialFood;
	public ImmutableDataFood bannedFood;

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
	public List<ImmutableDataFood> GetTwoMenuFoodChoices(FoodKeywords keyword, List<Allergies> _allergy){
		List<ImmutableDataFood> desiredFoodList = new List<ImmutableDataFood>();
		bool allergyFood = false;
		bool allergenAdded = false;
		int numOfWheatAllergen;
		int numOfDairyAllergen;
		int numOfPeanutAllergen;
		int numOfNoAllergen;

		if(RestaurantManager.Instance.isTutorial) {
			desiredFoodList.Add(DataLoaderFood.GetData("FoodFruitPlatter"));
			desiredFoodList.Add(DataLoaderFood.GetData("FoodPeanuts"));
			return desiredFoodList;
		}
		while(desiredFoodList.Count < 2){
			 numOfWheatAllergen = 0;
			 numOfDairyAllergen = 0;
			 numOfPeanutAllergen = 0;
			 numOfNoAllergen = 0;

			//checking to see if an allergy is present in the whole list
			for(int i = 0; i < menuList.Count; i++) {
				if(menuList[i].AllergyList.Contains(Allergies.Wheat)) {
					numOfWheatAllergen++;
				}
				else if(menuList[i].AllergyList.Contains(Allergies.Dairy)) {
					numOfDairyAllergen++;
				}
				else if(menuList[i].AllergyList.Contains(Allergies.Peanut)) {
					numOfPeanutAllergen++;
				}
				else if(menuList[i].AllergyList.Contains(Allergies.None)) {
					numOfNoAllergen++;
				}
			}
				if(numOfWheatAllergen == menuList.Count && _allergy.Contains(Allergies.Wheat)) {
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					return desiredFoodList;
				}
				else if(numOfDairyAllergen == menuList.Count && _allergy.Contains( Allergies.Dairy)) {
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					return desiredFoodList;
				}
				else if (numOfPeanutAllergen == menuList.Count && _allergy.Contains(Allergies.Peanut)) {
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					return desiredFoodList;
				}
				else if (numOfNoAllergen == menuList.Count) {
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					desiredFoodList.Add(menuList[Random.Range(0, menuList.Count)]);
					return desiredFoodList;
				}
			

			allergyFood = false;
			int rand = Random.Range(0,menuList.Count);
//				Debug.Log (menuList[rand].ID.ToString());
			if(!desiredFoodList.Contains(menuList[rand])){
				foreach(Allergies alg in menuList[rand].AllergyList){
					if(_allergy.Contains(alg)){
						allergyFood = true;	
					}
				}
				if(allergyFood){
					if(!allergenAdded){
						allergenAdded = true;
						desiredFoodList.Add(menuList[rand]);
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

	public void ChooseSpecialFood() {
		int rand = Random.Range(0, foodStockList.Count);
		specialFood = foodStockList[rand];
		while(foodStockList[rand] == specialFood) {
			rand = Random.Range(0, foodStockList.Count);
		}
		bannedFood = foodStockList[rand];
	}
}
