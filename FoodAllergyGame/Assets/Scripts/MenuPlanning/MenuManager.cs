using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : Singleton<MenuManager>{

	public int menuSize = 5;

	public GameObject selectedMenuPrefab;
	public GameObject foodStockPrefab;

	public List<Transform> selectedMenuParentList;
	public List<string> selectedMenuList;	// Internal aux list keeping track of current selection
	
	public bool AddFoodToMenuList(string foodID){
		if(selectedMenuList.Contains(foodID)){
			Debug.LogWarning("Menu already contains food");
			return false;
		}
		else if(selectedMenuList.Count >= menuSize){
			Debug.LogWarning("Menu list already at capacity");
			return false;
		}
		else{
			selectedMenuList.Add(foodID);
			// TODO finish populating food here

			return true;
		}
	}

}
