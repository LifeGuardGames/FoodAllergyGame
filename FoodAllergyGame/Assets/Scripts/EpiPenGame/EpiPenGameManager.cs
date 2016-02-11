using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{
	public int totalSteps = 9;
	public EpiPenGameUIManager UIManager;
	public List<Transform> pickSlots;
	public List<EpiPenGameToken> epiPenTokens;
	public Dictionary<int, int> submittedAnswers;
	public Transform activeDragParent;
	public Sprite lockedFinalSlotSprite;
	public Sprite emptyFinalSlotSprite;

	public GameObject leftButton;
	public GameObject rightButton;

	private int pickSlotPage = 0;
	private int pickSlotPageSize = 5;
	private List<bool> answers;

	void Start() {
		StartGame();
	}

	/// <summary>
	/// starts the game
	/// sets up the list and dictionary
	/// then calls remove pices and set up scene
	/// </summary>
	public void StartGame() {
		answers = new List<bool>();
		submittedAnswers = new Dictionary<int, int>();
		for(int i = 0; i < epiPenTokens.Count; i++) {
			answers.Add(true);
		}
		for(int i = 0; i < epiPenTokens.Count; i++) {
			submittedAnswers.Add(i,i);
		}

		foreach(Transform transform in pickSlots) {
			transform.gameObject.SetActive(true);
		}

		RemovePieces();
		SetUpScene();
	}

	/// <summary>
	/// Only called once 
	/// initial set up to remove a number of panels for the player to put back
	/// the number is equal to half their current tier
	/// to remove a piece we mark them as false in answers so that setUpScene will remove them from the slot and we remove it from the submitted anwsers
	/// </summary>
	private void RemovePieces() {
		//int tokensToRemove = TierManager.Instance.CurrentTier / 2;
		int tokensToRemove = 3;
		List<EpiPenGameToken> temp = new List<EpiPenGameToken>();
		for(int i = 0; i < epiPenTokens.Count; i++) {
			temp.Add(epiPenTokens[i]);
		}

		//Get a list of random values to remove
		foreach(int randomIndex in NumberUtils.UniqueRandomList(tokensToRemove, 0, totalSteps - 1)) {
			submittedAnswers.Remove(randomIndex);
			answers[randomIndex] = false;
		}
	}

	/// <summary>
	/// set up scene marks correct answers as locked so the player can not move them and removes incorrect answers based on the answer list
	/// </summary>
	private void SetUpScene() {
		for(int i = 0; i < answers.Count; i++) {
			if(answers[i]) {	// Correct answer
				epiPenTokens[i].IsLocked = true;
				epiPenTokens[i].transform.parent.GetComponent<Image>().sprite = lockedFinalSlotSprite;
            }
			else {              // Wrong answer
				epiPenTokens[i].transform.parent.GetComponent<Image>().sprite = emptyFinalSlotSprite;
				submittedAnswers.Remove(i);
				PlaceInAuxSlot(epiPenTokens[i]);
			}
		}

		// Remove the pick slots that are no longer valid and in use
		foreach(Transform transform in pickSlots) {
			if(transform.childCount == 0) {
				transform.gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// checks each answer in the submitted answers to see if it is correct
	/// a correct entry should look like i == i
	/// an incorrect entry looks like i != i
	/// it then fills the answer dictionary with true or false depending on the above statements
	/// We then call check for game over which will return true if all the entries in answers equal true
	/// otherwise we set up the scene again
	/// </summary>
	public void CheckAnswers() {
		// Make sure all the slots are populated
		if(submittedAnswers.Count != totalSteps) {
			Debug.LogWarning("Answers not complete");
			return;
		}

		for(int i = 0; i < submittedAnswers.Count; i++) {
			if(submittedAnswers[i] == i) {
				answers[i] = true;
			}
			else {
				answers[i] = false;
			}
		}
		if(CheckForGameOver()) {
			//You Win
			UIManager.ShowGameOver();
			//TODO preform game over logic here
		}
		else {
			SetUpScene();
		}
	}

	/// <summary>
	/// returns true if all the answers are correct else it will return false;
	/// </summary>
	public bool CheckForGameOver() {
		foreach(bool ans in answers) {
			if(!ans) {
				return false;
			}
		}
		return true;
	}
	
	/// <summary>
	/// Simply moves the in-play pieces to a slot and marks it as incorrect
	/// </summary>
	/// <param name="token"></param>
	public void PlaceInAuxSlot(EpiPenGameToken token) {
		foreach(Transform slot in pickSlots) {
			if(slot.childCount == 0) {
				token.transform.SetParent(slot);
				token.transform.localPosition = Vector3.zero;
				token.IsLocked = false;
				break;
			}
		}
	}

	#region Page button functions
	/*
	// Checks to see if the buttons need to appear
	public void RefreshButtonShowStatus() {
		// Check left most limit
		if(pickSlotPage == 0) {
			leftButton.SetActive(false);
		}
		else {
			leftButton.SetActive(true);
		}
		// Check right most limit
		if((pickSlotPage * pickSlotPageSize) + pickSlotPageSize >= foodStockList.Count) {
			rightButton.SetActive(false);
		}
		else {
			rightButton.SetActive(true);
		}
	}

	public void PageButtonClicked(bool isRightButton) {
		if(isRightButton) {
			pickSlotPage++;
		}
		else {
			pickSlotPage--;
		}

		ShowPage(pickSlotPage);
		RefreshButtonShowStatus();
	}

	// Either refreshes current page, or shows a new page given page number
	private void ShowPage(int foodStockPage) {
		// Destroy children beforehand
		foreach(Transform slot in currentFoodStockSlotList) {
			foreach(Transform child in slot) {  // Auto detect all/none children
				Destroy(child.gameObject);
			}
		}

		int startingIndex = foodStockPage * pickSlotPageSize;
		int endingIndex = startingIndex + pickSlotPageSize;

		for(int i = startingIndex; i < endingIndex; i++) {
			if(foodStockList.Count == i) {      // Reached the end of list
				break;
			}
			if(!selectedMenuStringList.Contains(foodStockList[i].ID)) {
				GameObject foodStockButton = GameObjectUtils.AddChildGUI(currentFoodStockSlotList[i % 4].gameObject, foodStockButtonPrefab);
				foodStockButton.GetComponent<FoodStockButton>().Init(foodStockList[i]);
			}
		}
	}
	*/
	#endregion
}
