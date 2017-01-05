using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DecoManager : Singleton<DecoManager>{
	public Dictionary <DecoTypes, GameObject> sceneObjects;
	private int decoPageSize = 4;
	private int currentDecoPage = 0;
	public GameObject decoButtonPrefab;
	public Transform grid;
	private DecoTypes currentTabType = DecoTypes.None;
	public GameObject leftButton;
	public GameObject rightButton;
	private List<ImmutableDataDecoItem> decoList;

	// Reference of all the deco loaders, dynamically assigned on start
	private Dictionary<DecoTypes, DecoLoader> decoLoaderHash = new Dictionary<DecoTypes, DecoLoader>();
	public Dictionary<DecoTypes, DecoLoader> DecoLoaderHash{
		get{ return decoLoaderHash; }
	}

	// Use these two references to tween and init them
	public ShowcaseController showcaseController;
	public TweenToggleDemux selectionPanelTween;
	public TweenToggle exitButtonTween;
	public GameObject tutObj1;
	public GameObject tutObj2;
	public GameObject tutObj3;
	public GameObject tutObj4;
	// For use with the uGUI layer ordering
	public Transform tabGroupActive;
	public Transform tabGroupInactive;
	public Sprite tabActiveSprite;
	public Sprite tabInactiveSprite;
	private Transform currentTabTransform;
	private Dictionary<string, Transform> tabGroupInactiveSearchTable;
	public bool isTutorial;
	public List<GameObject> tabNewTags;
	public GameObject dailySpecialTag;
	public PositionTweenToggle iapMenu;

	#region Generic functions
	public static bool IsDecoBought(string decoID){
		return DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID);
	}

	public static bool IsDecoActive(string decoID){
		return DataManager.Instance.GameData.Decoration.ActiveDeco.ContainsValue(decoID);
	}

	public bool IsDecoInRange(string decoID) {
		return (DataLoaderDecoItem.GetData(decoID).Tier <= TierManager.Instance.CurrentTier + 2) ? true : false;
	}

	public bool IsDecoUnlocked(string decoID) {
		return (DataLoaderDecoItem.GetData(decoID).Tier <= TierManager.Instance.CurrentTier) ? true : false;
	}
	
	public bool IsCategoryUnlocked(DecoTypes deco) {
		List<ImmutableDataDecoItem> decoTypeList = DataLoaderDecoItem.GetDecoDataByType(deco);
		foreach(ImmutableDataDecoItem decoData in decoTypeList) {
			if(IsDecoInRange(decoData.ID)) {
				return true;
			}
		}
		return false;
	}

	public static bool IsDecoRemoveAllowed(DecoTypes decoType) {
		switch(decoType) {
			case DecoTypes.Table:
			case DecoTypes.Kitchen:
			case DecoTypes.Floor:
				return false;
			case DecoTypes.FlyThru:
			case DecoTypes.Microwave:
			case DecoTypes.PlayArea:
			case DecoTypes.VIP:
				return true;
			default:
				Debug.LogWarning("Illegal deco type");
				return false;
		}
	}
	#endregion

	void Awake(){
		sceneObjects = new Dictionary<DecoTypes, GameObject>();

		// Position fixes for tweening
		GameObjectUtils.CopyRectTransform((RectTransform)tabGroupActive.transform, (RectTransform)tabGroupInactive.transform);

		// Populate the child list to search for tab changing order rendering
		tabGroupInactiveSearchTable = new Dictionary<string, Transform>();
		foreach(Transform child in tabGroupInactive){
			if(child.gameObject.activeSelf){
				tabGroupInactiveSearchTable.Add(child.name, child);
			}
		}
		foreach (string data in DataManager.Instance.GameData.Decoration.NewDeco) {
			switch(DataLoaderDecoItem.GetData(data).Type) {
				case DecoTypes.Table:
					tabNewTags[0].SetActive(true);
					break;
				case DecoTypes.Floor:
					tabNewTags[1].SetActive(true);
					break;
				case DecoTypes.Kitchen:
					tabNewTags[2].SetActive(true);
					break;
				case DecoTypes.VIP:
					tabNewTags[3].SetActive(true);
					break;
				case DecoTypes.PlayArea:
					tabNewTags[4].SetActive(true);
					break;
				case DecoTypes.FlyThru:
					tabNewTags[5].SetActive(true);
					break;
			}
		}
	}

	void Start(){
		if (!DataManager.Instance.GameData.Tutorial.IsDecoFingerTutDone && TierManager.Instance.CurrentTier == 2){
			isTutorial = true;
			tutObj1.SetActive(true);
		}
		// Start with table tab
		ChangeTab("Table");

		AnalyticsManager.Instance.TrackSceneEntered(SceneUtils.DECO);
	}

	// Populates the deco grid and returns the first unbought deco if any
	public ImmutableDataDecoItem PopulateDecoGrid(){
		// Delete any existing buttons in the grid
		foreach(Transform child in grid){
			Destroy(child.gameObject);
		}
		dailySpecialTag.SetActive(false);

		//creates a list of deco based on a type, to do this the dataloader first creates the list of all the items then sorts it by cost before returning it
		switch(currentTabType) {
			case DecoTypes.Table:
				decoList = new List<ImmutableDataDecoItem>();
				foreach(string deco in DataManager.Instance.GameData.Daily.RotTables) {
					decoList.Add(DataLoaderDecoItem.GetData(deco));
				}
				break;
			case DecoTypes.Kitchen:
				decoList = new List<ImmutableDataDecoItem>();
				foreach(string deco in DataManager.Instance.GameData.Daily.RotKitchen) {
					decoList.Add(DataLoaderDecoItem.GetData(deco));
				}
				break;
			case DecoTypes.Floor:
				decoList = new List<ImmutableDataDecoItem>();
				foreach(string deco in DataManager.Instance.GameData.Daily.RotFloors) {
					decoList.Add(DataLoaderDecoItem.GetData(deco));
				}
				break;
			case DecoTypes.Special:
				decoList = new List<ImmutableDataDecoItem>();
				decoList.Add(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Daily.SpeciDeco));
				dailySpecialTag.SetActive(true);
				break;
			case DecoTypes.IAP:
				decoList = DataLoaderDecoItem.GetDecoDataByType(DecoTypes.IAP);
                break;
			default:
				decoList = DataLoaderDecoItem.GetDecoDataByType(currentTabType);
				break;
		}
		
	
		ImmutableDataDecoItem firstUnboughtDeco = null;

		for(int i = 0; i < decoPageSize; i++){
			if(decoList.Count <= i + currentDecoPage * decoPageSize){		// Reached the end of list
				currentDecoPage--;
				break;
			}
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoList[i + currentDecoPage * decoPageSize].ID);
			GameObject decoButton = GameObjectUtils.AddChildGUI(grid.gameObject, decoButtonPrefab);
			decoButton.GetComponent<DecoButton>().Init(decoData);
			RefreshButtonShowStatus();

			if(firstUnboughtDeco == null && !IsDecoBought(decoData.ID)){
				firstUnboughtDeco = decoData;
			}
			if (currentTabType == DecoTypes.Special) {
				firstUnboughtDeco = DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Daily.SpeciDeco);
			}
		}
		return firstUnboughtDeco;
	}

	public void ShowCaseDeco(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		showcaseController.ShowInfo(decoData);
	}

	// Only certain items can be removed!
	public void RemoveDeco(DecoTypes type) {
		if(!IsDecoRemoveAllowed(type)) {
			Debug.LogError("Illegal remove call on deco type");
			return;
		}

		DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(type);

		// Refresh deco buttons of the update
		RefreshButtonState();

		// Enter view mode
		showcaseController.EnterViewMode();
		selectionPanelTween.Hide();
		exitButtonTween.Hide();

		StartCoroutine(RefreshItem(type));
		StartCoroutine(ExitViewMode());
	}

	// TODO Return false if you dont have enough money
	// Return decoID to null if you want to remove
	public void SetDeco(string decoID, DecoTypes decoType){
		if(IsDecoInitSanityCheck(decoID, decoType)){	// Sanity check
			if(decoID == null || IsDecoBought(decoID)) {
				//if(isTutroial && decoID == "PlayArea00"){
				if(isTutorial && decoID == "VIPBasic") {
					tutObj3.SetActive(false);
					isTutorial = false;
					DataManager.Instance.GameData.Tutorial.IsDecoFingerTutDone = true;
				}

				DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(decoType);

				if(decoID != null) {
					DataManager.Instance.GameData.Decoration.ActiveDeco.Add(decoType, decoID);
				}

				// Refresh deco buttons of the update
				RefreshButtonState();

				// Enter view mode
				showcaseController.EnterViewMode();
				selectionPanelTween.Hide();
				exitButtonTween.Hide();

				StartCoroutine(RefreshItem(decoType));
				StartCoroutine(ExitViewMode());
			}
			else {
				if(BuyItem(decoID)) {
					DataManager.Instance.SaveGameData();
				}
			}
		}
	}

	// Sanity check for parameters
	private bool IsDecoInitSanityCheck(string decoID, DecoTypes decoType) {
		if(decoID == null) {	// Attempting to remove decoration
			if(decoType == DecoTypes.None) {	// Check for illegal types
				Debug.LogError("Illegal type");
				return false;
			}
			if(!IsDecoRemoveAllowed(decoType)) {
				Debug.LogError("Decoration removal not permitted");
			}
		}
		else {
			if(DataLoaderDecoItem.GetData(decoID).Type != decoType) {
				Debug.LogError("Deco mismatch with type:" + decoID + " " + decoType.ToString());
				return false;
			}
		}
		return true;
	}

	private IEnumerator ExitViewMode(){
		yield return new WaitForSeconds(2.5f);
		showcaseController.ExitViewMode();
		selectionPanelTween.Show();
		exitButtonTween.Show();
	}

	private IEnumerator RefreshItem(DecoTypes deco){
		yield return new WaitForSeconds(.8f);
		DecoLoader loader = decoLoaderHash[deco];	// Get the loader script by type
		ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(deco);
		loader.LoadDeco(decoData, isPlayPoof: true);
	}

	private bool BuyItem(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		
		// Check if you have enough money, change cash if so, takes care of anim as well
		if(CashManager.Instance.DecoBuyCash(decoData.Cost) && decoData.Type != DecoTypes.IAP){
			if(decoData.Type == DecoTypes.Special) {
				DataManager.Instance.GameData.Decoration.CustomersBought.Add(decoData.ID);
			}
			DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, "");
			SetDeco(decoID, decoData.Type);
			AnalyticsManager.Instance.TrackDecoBought(decoID);
            return true;
		}
		else if (decoData.Type == DecoTypes.IAP) {
			if(DataManager.Instance.GameData.DayTracker.IAPCurrency >= decoData.IapPrice) {
				DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, "");
				SetDeco(decoID, decoData.Type);
				AnalyticsManager.Instance.TrackDecoBought(decoID);
				return true;
			}
			else {
				OpenIapMenu();
			}
			return false;
		}
		else{
			return false;
		}
	}

	public void ChangeTab(string tabName){
		currentDecoPage = 0;
		//if(isTutroial && tabName == "PlayArea" && tutObj1.activeSelf == true){
		if(isTutorial && tabName == "VIP" && tutObj1.activeSelf == true){
			tutObj1.SetActive(false);
			tutObj3.SetActive(true);
		}
		//else if (isTutroial && tabName != "PlayArea" && tutObj4.activeSelf != true){
		else if (isTutorial && tabName != "VIP" && tutObj4.activeSelf != true){
			tutObj1.SetActive(true);
			tutObj2.SetActive(false);
			tutObj3.SetActive(false);
		}

		if(currentTabType.ToString() != tabName){
			currentTabType = (DecoTypes)Enum.Parse(typeof(DecoTypes), tabName);
			Transform topSprite;

			// Reset any existing tabs to inactive parent
			if(currentTabTransform != null){
				currentTabTransform.SetParent(tabGroupInactive);
				topSprite = currentTabTransform.FindChild("Top");
				topSprite.GetComponent<Image>().sprite = tabInactiveSprite;
			}

			// Sort the ordering of the tab to active parent
			currentTabTransform = tabGroupInactiveSearchTable["Tab" + tabName];
			currentTabTransform.SetParent(tabGroupActive);
			topSprite = currentTabTransform.FindChild("Top");
			topSprite.GetComponent<Image>().sprite = tabActiveSprite;

			// Repopulate the grid and get the first unbought deco
			ImmutableDataDecoItem firstUnboughtDeco = PopulateDecoGrid();
			
			// Showcase the first buyable deco if there is any, if not show the lower tier decoration
			showcaseController.ShowInfo(firstUnboughtDeco != null ?
				firstUnboughtDeco.ID :
				DataLoaderDecoItem.GetDecoDataByType((DecoTypes)Enum.Parse(typeof(DecoTypes), tabName))[0].ID);
		}
	}

	public void ChangePage(bool isRight){
		if(isRight){
			currentDecoPage++;
		}
		else{
			if(currentDecoPage > 0){
				currentDecoPage--;
			}
		}
		PopulateDecoGrid();
	}

	public void RefreshButtonShowStatus(){
		// Check left most limit
		if(currentDecoPage == 0){
			leftButton.SetActive(false);
		}
		else{
			leftButton.SetActive(true);
		}
		// Check right most limit
		if((currentDecoPage * 4) + 4 >= decoList.Count){
			rightButton.SetActive(false);
		}
		else{
			rightButton.SetActive(true);
		}
	}

	public void RefreshButtonState(){
		grid.gameObject.BroadcastMessage("RefreshButtonState", SendMessageOptions.DontRequireReceiver);
	}

	public void OnExitButtonClicked(){
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: true);
	}

	public void OpenIapMenu() {
		iapMenu.Show();
	}
}
