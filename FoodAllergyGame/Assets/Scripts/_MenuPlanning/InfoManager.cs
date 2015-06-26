using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InfoManager : Singleton<InfoManager>{
	public GameObject foodStockButtonPrefab;
	public GameObject customerButtonPrefab;
	public GameObject grid;
	public PanelInfoController infoController;

	/// <summary>
	/// Called from the categories button
	/// Needs to be a string so unity is able to serialize this
	/// </summary>
	public void PopulateGridForType(string infoTypeString){
		InfoType type = (InfoType)Enum.Parse(typeof(InfoType), infoTypeString);
		switch(type){
		case InfoType.Food:
			// Instantiate list here
			foreach(ImmutableDataFood foodData in DataLoaderFood.GetDataList()){
				GameObject foodStockButton = 
					GameObjectUtils.AddChildGUI(grid, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodData);
			}

			// Show content
			StartManager.Instance.ShowInfoDetailDemux();
			break;
		case InfoType.Customer:
			// Instantiate list heres
			foreach(ImmutableDataCustomer customerData in DataLoaderCustomer.GetDataList()){
				GameObject customerButton = 
					GameObjectUtils.AddChildGUI(grid, customerButtonPrefab);
				customerButton.GetComponent<CustomerInfoButton>().Init(customerData);
			}

			// Show content
			StartManager.Instance.ShowInfoDetailDemux();
			break;
		}
	}
	public List<ImmutableDataFood> GetListForFood(){
		return DataLoaderFood.GetDataList();
	}
	
	public List<ImmutableDataCustomer> GetListForMonsters(){
		return DataLoaderCustomer.GetDataList();
	}

	public void ShowDetail(InfoType infoType, string ID){
		infoController.ShowInfo(infoType, ID);
	}

	public void ClearDetail(){
		// Disable info panel
		infoController.ToggleVisibility(false, InfoType.None);

		// Clear the grid
		foreach(Transform child in grid.transform){
			Destroy(child.gameObject);
		}
	}
}
