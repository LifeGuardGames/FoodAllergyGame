using UnityEngine;
using System.Collections.Generic;

public class MenuManager : Singleton<MenuManager>{

	private int menuSize;
	public GameObject foodStockButtonPrefab;

	public Transform selectedGrid;
	public Transform SelectedGrid { get { return selectedGrid; } }

	public EventPopupController eventPopController;
	public SlotBarController slotBarController;

	public TweenToggle trashTweenToggle;
	public MenuDragTrashSlot trashSlot;

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
	
	public GameObject tutFinger;

	public TweenToggle doneButtonTween;

	private bool isMenuTutAux = false;

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
		slotBarController.Init(menuSize);
		
		PopulateStockGrid();
		InitSanityCheck();
		
		// Show tut finger if it is not done
		tutFinger.SetActive(!DataManager.Instance.GameData.Tutorial.IsMenuPlanningFingerTutDone);
		if(!DataManager.Instance.GameData.Tutorial.IsMenuPlanningFingerTutDone) {
			isMenuTutAux = true;
            AnalyticsManager.Instance.TutorialFunnel("Entered menu picking tut");
		}
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

		// Populate the first page of the food stock
		for(int i = 0; i < foodStockPageSize; i++) {
			if(foodStockList.Count == i) {      // Reached the end of list
				break;
			}
			if(!selectedMenuStringList.Contains(foodStockList[i].ID)) {
				GameObject foodStockButton = GameObjectUtils.AddChildGUI(currentFoodStockSlotList[i].gameObject, foodStockButtonPrefab);
				foodStockButton.GetComponent<RectTransform>().localPosition = new Vector3(100f, -75f, 0);
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
		ImmutableDataFood foodData = DataLoaderFood.GetData(foodID);

		// Tick finger tutorial finished
		if(!DataManager.Instance.GameData.Tutorial.IsMenuPlanningFingerTutDone) {
			tutFinger.SetActive(false);
			DataManager.Instance.GameData.Tutorial.IsMenuPlanningFingerTutDone = true;
        }
		if(selectedMenuStringList.Contains(foodID)) {
			Debug.LogWarning("Menu already contains food");
			return false;
		}
		else if(slotBarController.ActivateSlots(foodData.Slots)) {
			// Add ID to aux string list
			selectedMenuStringList.Add(foodID);

			// Check full case
			if(slotBarController.IsSlotsFull()) {
				doneButtonTween.Show();
			}
			return true;
		}
		ParticleUtils.PlaySlotssFullFloaty(doneButtonTween.GetShowPos());
		return false;
	}

	public bool RemoveFoodFromMenuList(string foodID){
		ImmutableDataFood foodData = DataLoaderFood.GetData(foodID);

		if(selectedMenuStringList.Contains(foodID)) {
			selectedMenuStringList.Remove(foodID);
			slotBarController.DeactivateSlots(foodData.Slots);
			doneButtonTween.Hide();
			return true;
		}
		else {
			Debug.LogError("Removing a food that doesnt exist");
			return false;
		}
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

	public void OnMenuSelectionDone(){
		// Check to see if we have all selection slots filled
		if(slotBarController.IsSlotsFull()) {
			// Track in analytics
			AnalyticsManager.Instance.TrackMenuChoices(selectedMenuStringList);
			if(isMenuTutAux) {
				AnalyticsManager.Instance.TutorialFunnel("Finished menu picking tut");
			}

			FoodManager.Instance.GenerateMenu(selectedMenuStringList);
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT);
		}
		else{
			Debug.LogWarning("Menu not complete!");
		}
	}
}
