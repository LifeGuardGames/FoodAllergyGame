using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// TODO Nested list loop (menu x foodstock) in here, check run time
public class MenuManager : Singleton<MenuManager>{

	private int menuSize;

	public GameObject foodStockButtonPrefab;

	public AllergiesChartController allergiesChartController;
	public AllergiesChartController AllergiesChartController{
		get{ return allergiesChartController; }
	}

	public SelectedMenuController selectedMenuController;
	public GameObject eventDescription;

	private string currEvent;
	private ImmutableDataEvents currEventData;

	public TweenToggle trashTweenToggle;
	public MenuDragSlotTrash trashSlot;

	public List<Transform> currentFoodStockSlotList;			// Slots of the menu food stock
	private List<string> foodStockList = new List<string>();	// All the foods that are allowed for today

	private int foodStockPage = 0;
	private int foodStockPageSize = 4;
	public GameObject leftButton;
	public GameObject rightButton;

	private List<Transform> selectedMenuSlotList;

	// Internal aux list keeping track of current selection
	private List<string> selectedMenuStringList = new List<string>();
	public List<string> SelectedMenuStringList{
		get{ return selectedMenuStringList; }
	}

	public RectTransform dragAux;
	public RectTransform tweenAux;

	private int menuCost = 0;
	public Text menuCostText;
	public Text doneButtonCostText;
	public Animation textCashAnimation;
	public GameObject tutFinger;

	public TweenToggle doneButtonTween;

	void Start(){
		currEvent = DataManager.Instance.GetEvent();
		currEventData = DataLoaderEvents.GetData(currEvent);

//		eventDescription.SetActive(true);
//		ShowEventDescription();	// TODO taking this out for now

		// Load the number of slots from progress
		menuSize = TierManager.Instance.GetMenuSlots();
		selectedMenuController.Init(menuSize);
		allergiesChartController.Init(menuSize);

		ChangeMenuCost(0);	// Reset text to zero
		PopulateStockGrid();
		InitSanityCheck();
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

		// UNDONE Waiting for some debug stuff to be completed
		// Load the food stock set from the DataManager
//		foodStockList = DataManager.Instance.GameData.RestaurantEvent.MenuPlanningStock;

		// Sort the food stock list by price
		foodStockList.Sort((x,y) => DataLoaderFood.GetData(x).Cost.CompareTo(DataLoaderFood.GetData(y).Cost));

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

	#region Food stock page functions
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

		ShowPage(foodStockPage);
		RefreshButtonShowStatus();
	}

	// Used for the trash can, refresh current list
	public void RefreshCurrentPage(){
		ShowPage(foodStockPage);
	}

	// Either refreshes current page, or shows a new page given page number
	private void ShowPage(int foodStockPage){
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
	}
	#endregion
	
	public bool AddFoodToMenuList(string foodID){
		tutFinger.SetActive(false);
		if(selectedMenuStringList.Contains(foodID)){
			Debug.LogWarning("Menu already contains food");
			return false;
		}
		else if(selectedMenuStringList.Count > menuSize){
			Debug.LogWarning("Menu list already at capacity");
			return false;
		}
		else{
			// Add id to aux string list
			selectedMenuStringList.Add(foodID);
			allergiesChartController.UpdateChart();

			if(selectedMenuStringList.Count == menuSize){
				doneButtonTween.Show();
			}
			return true;
		}
	}

	public bool RemoveFoodFromMenuList(string foodID){
		// Soft remove - no error if doesnt find key
		bool isRemoved = selectedMenuStringList.Remove(foodID);
		allergiesChartController.UpdateChart();

		if(isRemoved){
			doneButtonTween.Hide();
		}
		return isRemoved;
	}

	public void ShowTrashCan(){
		trashTweenToggle.Show();
		trashSlot.enabled = true;
	}

	public void HideTrashCan(){
		trashTweenToggle.Hide();
		trashSlot.enabled = false;	// Disable script incase of drag slot overlay
	}

	public void OnBackButtonClicked(){
		TransitionManager.Instance.TransitionScene(SceneUtils.START);
	}

	public void ShowEventDescription(){
		eventDescription.GetComponentInChildren<Text>().text = eventDescription.GetComponent<Localize>().GetText(currEventData.ID);
	}

	public void CloseEventDescription(){
		eventDescription.SetActive(false);
	}

	public void ChangeMenuCost(int delta){
		menuCost += delta;
		textCashAnimation.Play();
		menuCostText.text = menuCost.ToString();
		doneButtonCostText.text = menuCost.ToString();
	}

	public void OnMenuSelectionDone(){
		// Check to see if we have all selection slots filled
		if(selectedMenuStringList.Count == menuSize){
			FoodManager.Instance.GenerateMenu(selectedMenuStringList, menuCost);
			TransitionManager.Instance.TransitionScene(SceneUtils.RESTAURANT);
		}
		else{
			Debug.LogWarning("Menu not complete!");
		}
	}
}
