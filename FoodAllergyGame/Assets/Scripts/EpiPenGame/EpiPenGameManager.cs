using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{
	public int totalSteps = 8;
	public EpiPenGameUIManager UIManager;
	
	public List<Transform> finalSlotList;
	private List<int> allPickTokens = new List<int>();

	public List<Transform> currentPickSlotList;
	public GameObject tokenPrefab;

//	public List<EpiPenGameToken> epiPenTokens;
//	public Dictionary<int, int> submittedAnswers;
	public Transform activeDragParent;
	public Sprite lockedFinalSlotSprite;
	public Sprite emptyFinalSlotSprite;

	public GameObject leftButton;
	public GameObject rightButton;

	private int pickSlotPage = 0;
	private int pickSlotPageSize = 5;
	private List<bool> answers;

	private int attempts = 0;

	void Start() {
		StartGame();
	}

	/// <summary>
	/// Add all the final tokens to the top and keep internal list of all the pick tokens
	/// </summary>
	public void StartGame() {
		//int tokensToPick = TierManager.Instance.CurrentTier / 2;
		int tokensToPick = 6;

		// Populate the tokens to remove by order number
		foreach(int randomIndex in NumberUtils.UniqueRandomList(tokensToPick, 0, totalSteps - 1)) {
			allPickTokens.Add(randomIndex);
        }

		// Add all the children that are not in the pick token list
		for(int i = 0; i < totalSteps; i++) {
			if(!allPickTokens.Contains(i)) {
				GameObject go = GameObjectUtils.AddChildGUI(finalSlotList[i].gameObject, tokenPrefab);
				go.GetComponent<EpiPenGameToken>().Init(i, true);
            }
		}

		bool perfectCheck = RefreshTokenState();
		if(perfectCheck) {
			Debug.LogError("Perfect on start!");
		}

		RefreshButtonShowStatus();
		ShowPage(0);
    }

	/// <summary>
	/// Checks each answer in the submitted answers to see if it is correct
	/// </summary>
	public void CheckAnswerButtonClicked() {
		attempts++;
        if(RefreshTokenState()) {
			//You Win
			UIManager.ShowGameOver(attempts);
			//TODO preform game over logic here
		}
		else {
			RefreshButtonShowStatus();
			ShowPage(0);
		}
	}

	/// <summary>
	/// Returns true if every finalSlotList has a token and its order matches index
	/// Otherwise, it updates the slot statuses and UI accordingly
	/// </summary>
	private bool RefreshTokenState() {
		bool isPerfect = true;
		for(int i = 0; i < finalSlotList.Count; i++) {
			if(finalSlotList[i].childCount > 1) {
				if(finalSlotList[i].GetComponentInChildren<EpiPenGameToken>().order == i) {
					finalSlotList[i].GetComponentInChildren<EpiPenGameToken>().IsLocked = true;		// Lock the token

					if(allPickTokens.Remove(i)) {       // Soft remove
						finalSlotList[i].GetComponent<Image>().sprite = lockedFinalSlotSprite;
					}
				}
				else {
					Destroy(finalSlotList[i].GetComponentInChildren<EpiPenGameToken>().gameObject);
					finalSlotList[i].GetComponent<Image>().sprite = emptyFinalSlotSprite;
					isPerfect = false;
				}
			}
			else {
				finalSlotList[i].GetComponent<Image>().sprite = emptyFinalSlotSprite;
				isPerfect = false;
			}
		}
		return isPerfect;
	}
	
	/// <summary>
	/// Simply moves the in-play pieces to a slot and marks it as incorrect
	/// </summary>
	/// <param name="token"></param>
	public void PlaceInAuxSlot(EpiPenGameToken token) {
		/*
		foreach(Transform slot in allPickSlots) {
			if(slot.childCount == 0) {
				token.transform.SetParent(slot);
				token.transform.localPosition = Vector3.zero;
				token.IsLocked = false;
				break;
			}
		}
		*/
	}

	#region Page button functions
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
		if((pickSlotPage * pickSlotPageSize) + pickSlotPageSize >= allPickTokens.Count) {
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
	private void ShowPage(int pickSlotPage) {
		// Destroy children beforehand
		foreach(Transform slot in currentPickSlotList) {
			foreach(Transform child in slot) {  // Auto detect all/none children
				Destroy(child.gameObject);
			}
		}
		
		int startingIndex = pickSlotPage * pickSlotPageSize;
		int endingIndex = startingIndex + pickSlotPageSize;

		for(int i = startingIndex; i < endingIndex; i++) {
			if(allPickTokens.Count == i) {      // Reached the end of list
				break;
			}
			GameObject slotToken = GameObjectUtils.AddChildGUI(currentPickSlotList[i % pickSlotPageSize].gameObject, tokenPrefab);
            slotToken.GetComponent<EpiPenGameToken>().Init(allPickTokens[i], false);
		}
	}
	#endregion
}
