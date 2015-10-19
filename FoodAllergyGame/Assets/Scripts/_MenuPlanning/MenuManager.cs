using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>{

	private int menuSize;
	public GameObject foodStockButtonPrefab;

	public EventPopupController eventPopController;
	public SelectedMenuController selectedMenuController;
	public TweenToggle trashTweenToggle;
	public MenuDragSlotTrash trashSlot;

	public List<Transform> currentFoodStockSlotList;			// Slots of the menu food stock
	private List<ImmutableDataFood> foodStockList = new List<ImmutableDataFood>();	// All the foods that are allowed for today
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
	public int availableSlots;

	public TweenToggle doneButtonTween;

	void Start(){
		// Show the day's event notification
		if(!DataManager.Instance.IsDebug){
			eventPopController.Init(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));
		}
		else{
			eventPopController.Init(DataLoaderEvents.GetData("Event00"));
		}

		// Load the number of slots from progress
		menuSize = TierManager.Instance.GetMenuSlots();
		selectedMenuController.Init(menuSize);
		availableSlots = menuSize;

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
		if(!DataManager.Instance.IsDebug){
			// Load the food stock set from FoodManager
			foodStockList = FoodManager.Instance.FoodStockList;
		}
		else{
			foodStockList = DataLoaderFood.GetDataList();
		}

		// Sort the food stock list by price
		foodStockList.Sort((x,y) => x.Cost.CompareTo(y.Cost));

		// Populate the first page of the food stock
		for(int i = 0; i < foodStockPageSize; i++){
			if(foodStockList.Count == i){		// Reached the end of list
				break;
			}
			if(!selectedMenuStringList.Contains(foodStockList[i].ID)){
				GameObject foodStockButton = GameObjectUtils.AddChildGUI(currentFoodStockSlotList[i].gameObject, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodStockList[i]);
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
			if(!selectedMenuStringList.Contains(foodStockList[i].ID)){
				GameObject foodStockButton = GameObjectUtils.AddChildGUI(currentFoodStockSlotList[i % 4].gameObject, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodStockList[i]);
			}
		}
	}
	#endregion
	
	public bool AddFoodToMenuList(string foodID){
		tutFinger.SetActive(false);
		if (selectedMenuStringList.Contains(foodID)) {
			Debug.LogWarning("Menu already contains food");
			return false;
		}
		else if (selectedMenuStringList.Count > menuSize) {
			Debug.LogWarning("Menu list already at capacity");
			return false;
		}

		else if (availableSlots < DataLoaderFood.GetData(foodID).Slots) {
			Debug.LogWarning("Slots Full");
			return false;
		}

		else if (DataManager.Instance.GameData.Cash.CurrentCash < DataLoaderFood.GetData(foodID).Cost) {
			Debug.LogWarning("Not enough cash");
			return false;
		}

		else {
			// Add ID to aux string list
			selectedMenuStringList.Add(foodID);
			availableSlots -= DataLoaderFood.GetData(foodID).Slots;
			if (availableSlots == 0) {
				doneButtonTween.Show();
			}
			return true;
		}
	}

	public bool RemoveFoodFromMenuList(string foodID){
		// Soft remove - no error if doesnt find key
		bool isRemoved = selectedMenuStringList.Remove(foodID);

		if(isRemoved){
			availableSlots += DataLoaderFood.GetData(foodID).Slots;
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
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
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
			Analytics.CustomEvent("Menu", new Dictionary<string, object>{
				{"Menu Items", selectedMenuStringList}
			});
			FoodManager.Instance.GenerateMenu(selectedMenuStringList, menuCost);
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT);
		}
		else{
			Debug.LogWarning("Menu not complete!");
		}
	}
}
