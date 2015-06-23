using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : Singleton<MenuManager>{

	public int menuSize = 5;

	public GameObject selectedMenuButtonPrefab;
	public GameObject foodStockButtonPrefab;

	public PanelInfoController panelInfoController;
	public AllergiesChartController allergiesChartController;
	public GameObject foodStockGrid;

	public List<Transform> selectedMenuParentList;
	public List<string> selectedMenuList;	// Internal aux list keeping track of current selection

	void Start(){
		PopulateStockGrid();
	}

	public void PopulateStockGrid(){
		foreach(ImmutableDataFood foodData in DataLoaderFood.GetDataList()){
			GameObject foodStockButton = 
				GameObjectUtils.AddChildGUI(foodStockGrid, foodStockButtonPrefab);
			foodStockButton.GetComponent<FoodStockButton>().Init(foodData);
		}
	}
	
	public bool AddFoodToMenuList(string foodID){
		// Display info in panel
		panelInfoController.ShowInfo(InfoType.Food, foodID);

		if(selectedMenuList.Contains(foodID)){
			Debug.LogWarning("Menu already contains food");
			return false;
		}
		else if(selectedMenuList.Count >= menuSize){
			Debug.LogWarning("Menu list already at capacity");
			return false;
		}
		else{
			// Add id to selectedMenuList
			selectedMenuList.Add(foodID);

			// Spawn copy of selected menu prefab here with the food
			Transform parentToPopulate = null;
			foreach(Transform parent in selectedMenuParentList){	// Look for empty space in parents
				if(parent.childCount == 0){
					parentToPopulate = parent;
					break;
				}
			}
			GameObject menuButtonObject = 
				GameObjectUtils.AddChildGUI(parentToPopulate.gameObject, selectedMenuButtonPrefab);

			menuButtonObject.GetComponent<SelectedMenuButton>().Init(foodID);
			allergiesChartController.UpdateChart(selectedMenuList);
			return true;
		}
	}

	public void RemoveFoodFromMenuList(string foodID){
		string stringToRemove = null;

		// Remove the string from the aux list
		foreach(string menuFoodID in selectedMenuList){
			if(string.Equals(menuFoodID, foodID)){
				stringToRemove = foodID;
				break;
			}
		}

		// Try to remove the string from the menu list
		if(!selectedMenuList.Remove(stringToRemove)){
			Debug.LogError("Error in removing food : " + stringToRemove);
		}

		// Remove the string from the parent transform
		foreach(Transform parent in selectedMenuParentList){
			Transform transformToRemove = parent.FindChild(stringToRemove);
			if(transformToRemove != null){
				Destroy(transformToRemove.gameObject);
				break;
			}
		}

		allergiesChartController.UpdateChart(selectedMenuList);
	}

	public void OnMenuSelectionDone(){
		// Check to see if we have all selection slots filled
		if(selectedMenuList.Count == menuSize){
			FoodManager.Instance.GenerateMenu(selectedMenuList);
			TransitionManager.Instance.TransitionScene(SceneUtils.RESTAURANT);
		}
		else{
			Debug.LogWarning("Menu not complete!");
		}
	}
}
