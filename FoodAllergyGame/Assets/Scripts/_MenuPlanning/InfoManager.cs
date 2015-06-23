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
			// TODO Watch out for load demux break
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
			// TODO Watch out for load demux break
			foreach(ImmutableDataCustomer customerData in DataLoaderCustomer.GetDataList()){
				GameObject customerButton = 
					GameObjectUtils.AddChildGUI(grid, customerButtonPrefab);
				customerButton.GetComponent<CustomerButton>().Init(customerData);
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
}
