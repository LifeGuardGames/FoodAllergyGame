using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// TODO Nested list loop (menu x foodstock) in here, check run time
public class MenuManager : Singleton<MenuManager>{

	public int menuSize;

	public GameObject foodStockButtonPrefab;

	public MenuPanelInfoController menuPanelInfoController;
	public AllergiesChartController allergiesChartController;
	public GameObject foodStockGrid;
	public GameObject EventDescription;
	public string currEvent;

	public List<string> foodStockList;			// All the foods that are allowed for today
	public List<Transform> currentFoodStockSlotList;

	public int foodStockPage = 0;
	public int foodStockPageSize = 4;
	public GameObject leftButton;
	public GameObject rightButton;

	public List<Transform> selectedMenuSlotList;
	public List<string> selectedMenuStringList;	// Internal aux list keeping track of current selection

	public Transform dragAux;

	void Start(){
		currEvent = DataManager.Instance.GetEvent();
		InitSanityCheck();

		EventDescription.SetActive(true);
		ShowEventDescription();

		// TODO Load the number of slots from DataManager
		menuSize = 4;

		//////////////////////////////
	
		PopulateStockGrid();
	}

	// Check certain values to see if they are consistent
	private void InitSanityCheck(){
		if(foodStockPageSize != currentFoodStockSlotList.Count){
			Debug.LogError("Page size does not match up with food stock slot count");
		}

		// Add more here when needed

	}

	public void PopulateStockGrid(){
		foodStockList.Add("Food00");
		foodStockList.Add("Food01");
		foodStockList.Add("Food02");
		foodStockList.Add("Food03");
		foodStockList.Add("Food04");
		foodStockList.Add("Food05");
		foodStockList.Add("Food06");
		foodStockList.Add("Food07");
		foodStockList.Add("Food08");
		foodStockList.Add("Food09");

		// TODO Load the food stock set from the DataManager
//		foodStockList = ...

		for(int i = 0; i < foodStockPageSize; i++){
			if(foodStockList.Count == i){		// Reached the end of list
				break;
			}
			if(!selectedMenuStringList.Contains(foodStockList[i])){
				ImmutableDataFood foodData = DataLoaderFood.GetData(foodStockList[i]);
				GameObject foodStockButton = GameObjectUtils.AddChildGUI(currentFoodStockSlotList[i].gameObject, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodData);
			}
		}

		RefreshButtonShowStatus();
	}

	// Checks to see if the buttons need to appear
	public void RefreshButtonShowStatus(){
		// Check left most limit
		if(foodStockPage == 0){
			leftButton.SetActive(false);
		}
		else{
			leftButton.SetActive(true);
		}
		// Check right most limit
		if((foodStockPage * foodStockPageSize) + foodStockPageSize >= foodStockList.Count){
			rightButton.SetActive(false);
		}
		else{
			rightButton.SetActive(true);
		}
	}

	public void PageButtonClicked(bool isRightButton){
		if(isRightButton){
			foodStockPage++;
		}
		else{
			foodStockPage--;
		}

		// Destroy children beforehand
		foreach(Transform slot in currentFoodStockSlotList){
			foreach(Transform child in slot){	// Auto detect all/none children
				Destroy(child.gameObject);
			}
		}

		int startingIndex = foodStockPage * foodStockPageSize;
		int endingIndex = startingIndex + foodStockPageSize;

		for(int i = startingIndex; i < endingIndex; i++){
			if(foodStockList.Count == i){		// Reached the end of list
				break;
			}
			if(!selectedMenuStringList.Contains(foodStockList[i])){
				ImmutableDataFood foodData = DataLoaderFood.GetData(foodStockList[i]);
				GameObject foodStockButton = GameObjectUtils.AddChildGUI(currentFoodStockSlotList[i % 4].gameObject, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodData);
			}
		}
		RefreshButtonShowStatus();
	}
	
	public bool AddFoodToMenuList(string foodID){
		// Display info in panel
//		panelInfoController.ShowInfo(InfoType.Food, foodID);

		if(selectedMenuStringList.Contains(foodID)){
			Debug.LogWarning("Menu already contains food");
			return false;
		}
		else if(selectedMenuStringList.Count >= menuSize){
			Debug.LogWarning("Menu list already at capacity");
			return false;
		}
		else{
			// Add id to aux string list
			selectedMenuStringList.Add(foodID);

//			allergiesChartController.UpdateChart(selectedMenuStringList);
			return true;
		}
	}

	public void RemoveFoodFromMenuList(string foodID){
		// Soft remove - no error if doesnt find key
		selectedMenuStringList.Remove(foodID);

//		allergiesChartController.UpdateChart(selectedMenuStringList);
	}

	public void OnMenuSelectionDone(){
		// Check to see if we have all selection slots filled
		if(selectedMenuStringList.Count == menuSize){
			FoodManager.Instance.GenerateMenu(selectedMenuStringList);
			TransitionManager.Instance.TransitionScene(SceneUtils.TUTSCENE);
		}
		else{
			Debug.LogWarning("Menu not complete!");
		}
	}

	public void ShowEventDescription(){
		EventDescription.GetComponentInChildren<Text>().text = EventDescription.GetComponent<Localize>().GetText(DataLoaderEvents.GetData(currEvent).ID);
	}

	public void CloseEventDescription(){
		EventDescription.SetActive(false);
	}

	public void ShowFoodInfo(string foodID){
		menuPanelInfoController.Show(foodID);
	}

	public void HideFoodInfo(){
		menuPanelInfoController.Hide();
	}
}
