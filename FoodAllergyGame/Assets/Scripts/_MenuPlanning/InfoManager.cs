using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfoManager : Singleton<InfoManager> {
	public enum InfoType{
		Food,
		Customer
	}

	public TweenToggleDemux startDemux;
	public TweenToggleDemux infoCategoriesDemux;
	public TweenToggleDemux infoDetailDemux;

	public GameObject foodStockButtonPrefab;
	public GameObject customerButtonPrefab;
	public GameObject grid;
	
	public void ShowStartDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Hide();
		startDemux.Show();
	}
	
	public void ShowInfoCategoriesDemux(){
		infoDetailDemux.Hide();
		infoCategoriesDemux.Show();
		startDemux.Hide();
	}
	
	public void ShowInfoDetailDemux(){
		infoDetailDemux.Show();
		infoCategoriesDemux.Hide();
		startDemux.Hide();
	}

	public void PopulateGridForType(InfoType infoType){
		switch(infoType){
		case InfoType.Food:
			// Instantiate list here
			// TODO Watch out for load demux break
			foreach(ImmutableDataFood foodData in DataLoaderFood.GetDataList()){
				GameObject foodStockButton = 
					GameObjectUtils.AddChildGUI(grid, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodData);
			}

			// Show content
			ShowInfoDetailDemux();
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
			ShowInfoDetailDemux();
			break;
		}
	}
	public List<ImmutableDataFood> GetListForFood(){
		return DataLoaderFood.GetDataList();
	}
	
	public List<ImmutableDataCustomer> GetListForMonsters(){
		return DataLoaderCustomer.GetDataList();
	}

	public void ShowDetail(InfoType infoType, string id){

	}
}
