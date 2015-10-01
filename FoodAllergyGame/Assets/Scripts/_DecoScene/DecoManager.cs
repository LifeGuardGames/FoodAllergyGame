using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public class DecoManager : Singleton<DecoManager>{

	public List<SpriteRenderer> tableList;
	public List<SpriteRenderer> kitchenList;
	public Dictionary <DecoTypes, GameObject> sceneObjects;
	private int decoPageSize = 4;
	private int currentDecoPage = 0;
	public GameObject decoButtonPrefab;
	public Transform grid;
	private DecoTypes currentTabType = DecoTypes.Table;
	public GameObject leftButton;
	public GameObject rightButton;
	private List<ImmutableDataDecoItem> decoList;

	// Reference of all the deco loaders, dynamically assigned on start
	private Dictionary<DecoTypes, DecoLoader> decoLoaderHash = new Dictionary<DecoTypes, DecoLoader>();
	public Dictionary<DecoTypes, DecoLoader> DecoLoaderHash{
		get{ return decoLoaderHash; }
	}

	public GameObject decoTutorial;
	public DecoCameraTween cameraTween;

	// Use these two references to tween and init them
	public ShowcaseController showcaseController;
	public TweenToggleDemux selectionPanelTween;
	public TweenToggle exitButtonTween;
	public TweenToggle backButtonTween;	// Exits out of view mode
	public Animation backButtonPulseAnim;

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
	}

	void Start(){
		// Start with table tab
		ChangeTab("Table");
	}

	public void PopulateDecoGrid(){
		// Delete any existing buttons in the grid
		foreach(Transform child in grid){
			Destroy(child.gameObject);
		}
		//creates a list of deco based on a type, to do this the dataloader first creates the list of all the items then sorts it by type and cost before returning it
		decoList = DataLoaderDecoItem.GetDecoDataByType(currentTabType);

		for(int i = 0; i < decoPageSize; i++){
			if(decoList.Count <= i + currentDecoPage * decoPageSize){		// Reached the end of list
				currentDecoPage--;
				break;
			}
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoList[i + currentDecoPage * decoPageSize].ID);
			GameObject decoButton = GameObjectUtils.AddChildGUI(grid.gameObject, decoButtonPrefab);
			decoButton.GetComponent<DecoButton>().Init(decoData);
			RefreshButtonShowStatus();
		}
	}

	public void ShowcaseDeco(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		showcaseController.ShowInfo(decoData);
		Invoke("PulseBackButton", 2.5f);
	}

	private void PulseBackButton(){
		backButtonPulseAnim.Play();
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
			backButtonTween.Show();

			StartCoroutine(RefreshItem(decoData.Type));
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
				// HACK for prototyping only
				if(!DataManager.Instance.GameData.Decoration.DecoTut.Contains(decoData.Type)){
					decoTutorial.GetComponent<DecoTutorialController>().Init(decoData.Type);
					Invoke("ShowTutorial", 1.5f);
				}
				DataManager.Instance.SaveGameData();
			}
		}
	}

	private IEnumerator RefreshItem(DecoTypes deco){
		yield return new WaitForSeconds(.8f);
		DecoLoader loader = decoLoaderHash[deco];	// Get the loader script by type
		ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(deco);
		loader.LoadDeco(decoData, isPlayPoof: true);
	}

	public void ShowTutorial(){
		decoTutorial.SetActive(true);
	}

	private bool BuyItem(string decoID){
		if(DataLoaderDecoItem.GetData(decoID).Cost < DataManager.Instance.GameData.Cash.CurrentCash){
			DataManager.Instance.GameData.Cash.CurrentCash -= DataLoaderDecoItem.GetData(decoID).Cost;
			Debug.Log (HUDAnimator.Instance.name);
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

	public void ResetCamera(){
//		cameraTween.TweenCamera(DecoTypes.None);
	}

	public void ChangeTab(string tabName){
		currentDecoPage = 0;
		currentTabType = (DecoTypes)Enum.Parse(typeof(DecoTypes), tabName);
//		cameraTween.TweenCamera(currentTabType);
		PopulateDecoGrid();

		// Showcase the first deco
		showcaseController.ShowInfo(tabName + "00");
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

	// Exit view mode
	public void OnBackButtonClicked(){
		showcaseController.ExitViewMode();
		selectionPanelTween.Show();
		exitButtonTween.Show();
		backButtonTween.Hide();
		backButtonPulseAnim.Stop();
	}

	public void OnExitButtonClicked(){

		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
	}
}
