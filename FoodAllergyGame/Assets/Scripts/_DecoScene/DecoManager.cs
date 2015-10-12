using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;

public class DecoManager : Singleton<DecoManager>{
	public List<SpriteRenderer> tableList;
	public List<SpriteRenderer> kitchenList;
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

	// For use with the uGUI layer ordering
	public Transform tabGroupActive;
	public Transform tabGroupInactive;
	public Sprite tabActiveSprite;
	public Sprite tabInactiveSprite;
	private Transform currentTabTransform;
	private Dictionary<string, Transform> tabGroupInactiveSearchTable;

	#region Static functions
	public static bool IsDecoBought(string decoID){
		return DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID);
	}

	public static bool IsDecoActive(string decoID){
		return DataManager.Instance.GameData.Decoration.ActiveDeco.ContainsValue(decoID);
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
	}

	void Start(){
		// Start with table tab
		ChangeTab("Table");
	}

	// Populates the deco grid and returns the first unbought deco if any
	public ImmutableDataDecoItem PopulateDecoGrid(){
		// Delete any existing buttons in the grid
		foreach(Transform child in grid){
			Destroy(child.gameObject);
		}
		//creates a list of deco based on a type, to do this the dataloader first creates the list of all the items then sorts it by cost before returning it
		decoList = DataLoaderDecoItem.GetDecoDataByType(currentTabType);
		Debug.Log("count " + decoList.Count);
		ImmutableDataDecoItem firstUnboughtDeco = null;

		for(int i = 0; i < decoPageSize; i++){
			Debug.Log(i);
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
		}
		return firstUnboughtDeco;
	}

	public void ShowCaseDeco(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		showcaseController.ShowInfo(decoData);
	}

//	public bool PreviewDeco(string decoID){
		// TODO for down the road, Save an aux dummy instance and delete it when you exit preview mode.
		// Dont touch datamanager though
//	}

	// TODO Return false if you dont have enough money
	public void SetDeco(string decoID){
		if(IsDecoBought(decoID)){
			// Cache local data
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
			DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(decoData.Type);
			DataManager.Instance.GameData.Decoration.ActiveDeco.Add(decoData.Type, decoID);

			// Refresh deco buttons of the update
			RefreshButtonState();

			// Enter view mode
			showcaseController.EnterViewMode();
			selectionPanelTween.Hide();
			exitButtonTween.Hide();

			StartCoroutine(RefreshItem(decoData.Type));
			StartCoroutine(ExitViewMode());
		}
		else{
			if(BuyItem(decoID)){
				ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
				switch(decoData.Type){
				case DecoTypes.PlayArea:
					DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTP");
					DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
					break;
				default:
					break;
				}

				DataManager.Instance.SaveGameData();
			}
		}
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
		if(DataLoaderDecoItem.GetData(decoID).Cost < DataManager.Instance.GameData.Cash.CurrentCash){
			DataManager.Instance.GameData.Cash.CurrentCash -= DataLoaderDecoItem.GetData(decoID).Cost;
			HUDAnimator.Instance.CoinsEarned(-DataLoaderDecoItem.GetData(decoID).Cost, GameObject.Find ("ButtonBuy").transform.position);
			Analytics.CustomEvent("Item Bought", new Dictionary<string, object>{
				{"Item: ", decoID}
			});
			DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, "");
			SetDeco(decoID);
			return true;
		}
		else{
			return false;
		}
	}

	public void ChangeTab(string tabName){
		currentDecoPage = 0;

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
			
			// Showcase the first buyable deco if there is any
			showcaseController.ShowInfo(firstUnboughtDeco != null ? firstUnboughtDeco.ID : tabName + "00");
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
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
	}
}
