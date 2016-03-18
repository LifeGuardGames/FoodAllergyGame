using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{
	public int totalSteps = 8;
	public EpiPenGameUIManager UIManager;
	
	public List<EpiPenGameSlot> finalSlotList;
	private List<int> allPickTokens;

	public List<EpiPenGameSlot> currentPickSlotList;
	public GameObject tokenPrefab;
	
	public Transform activeDragParent;
	public Sprite lockedFinalSlotSprite;
	public Sprite emptyFinalSlotSprite;

	public GameObject leftButton;
	public GameObject rightButton;

	private int pickSlotPage = 0;
	private int pickSlotPageSize = 5;
	private List<bool> answers;
	public bool isGameover = false;
	public RectTransform animationTokenParent;
	public bool isTutorial;
	public GameObject tutFingerPrefab;
	private int attempts = 0;

	void Start() {
		if(DataManager.Instance.IsDebug) {
			isTutorial = Constants.GetConstant<bool>("IsEpiPenTutorialDone");
		}
		else {
			if(DataManager.Instance.GameData.Tutorial.IsEpiPenGameTutorialDone) {
				isTutorial = false;
			}
			else {
				isTutorial = true;
			}
		}
		StartGame();
	}

	/// <summary>
	/// Add all the final tokens to the top and keep internal list of all the pick tokens
	/// </summary>
	public void StartGame() {
		UIManager.StartGame();

		attempts = 0;
		allPickTokens = new List<int>();

		// Destroy children beforehand
		foreach(EpiPenGameSlot slot in finalSlotList) {
			slot.ClearToken();
		}

		// Need to wait one frame for everything to destroy
		StartCoroutine(StartHelper());
    }

	private IEnumerator StartHelper() {
		// Wait one frame, wait for everything to destroy
		yield return 0;

		//int tokensToPick = TierManager.Instance.CurrentTier / 2;
		int tokensToPick = 6;
		if(!isTutorial) {
			// Populate the tokens to remove by order number
			foreach(int randomIndex in NumberUtils.UniqueRandomList(tokensToPick, 0, totalSteps - 1)) {
				allPickTokens.Add(randomIndex);
			}
		}
		else {
			allPickTokens.Add(0);
			allPickTokens.Add(3);
			allPickTokens.Add(7);
			allPickTokens.Add(5);
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
		
		ShowPage(0);
		if(isTutorial) {
			StartTutorial();
		}
	}

	/// <summary>
	/// Further token logic checking here
	/// </summary>
	public void TokenPlaced() {
		// Checks if all final slots are filled, show the check answer button if all filled
		int placedTokenCount = 0;
        foreach(EpiPenGameSlot slot in finalSlotList) {
			if(slot.GetComponentInChildren<EpiPenGameToken>() != null) {
				placedTokenCount++;
			}
		}
		UIManager.CheckButtonToggle(placedTokenCount == finalSlotList.Count ? true : false);
	}

	/// <summary>
	/// Checks each answer in the submitted answers to see if it is correct
	/// </summary>
	public void CheckAnswerButtonClicked() {
		UIManager.CheckButtonToggle(false);

		// Check if all the tokens are filled
		foreach(EpiPenGameSlot slot in finalSlotList) {
			if(slot.GetToken() == null) {
				Debug.LogWarning("Final slots not filled");
				return;
			}
		}

		attempts++;
        if(RefreshTokenState()) {
			//You Win
			AnimateEnding();
			//TODO preform game over logic here
		}
		else {
			ShowPage(0, isCheckingAnswer: true);
		}
	}

	public void GameOverButtonClicked() {
		// Temp stuff
		UIManager.gameOverTween.Hide();
		StartGame();
	}

	/// <summary>
	/// Returns true if every finalSlotList has a token and its order matches index
	/// Otherwise, it updates the slot statuses and UI accordingly
	/// </summary>
	private bool RefreshTokenState() {
		bool isPerfect = true;
		for(int i = 0; i < finalSlotList.Count; i++) {
			if(finalSlotList[i].GetToken() != null) {
				if(finalSlotList[i].GetToken().order == i) {
					finalSlotList[i].GetToken().IsLocked = true;		// Lock the token

					if(allPickTokens.Remove(i)) {       // Soft remove
						finalSlotList[i].GetComponent<Image>().sprite = lockedFinalSlotSprite;
					}
				}
				else {
					Destroy(finalSlotList[i].GetToken().gameObject);
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

	#region Page button functions
	public void PageButtonClicked(bool isRightButton) {
		if(isRightButton) {
			pickSlotPage++;
		}
		else {
			pickSlotPage--;
		}

		ShowPage(pickSlotPage);
	}

	// Either refreshes current page, or shows a new page given page number
	private void ShowPage(int incomingPickSlotPage, bool isCheckingAnswer = false) {
		// Destroy children beforehand
		foreach(EpiPenGameSlot slot in currentPickSlotList) {
			slot.ClearToken();
		}

		pickSlotPage = incomingPickSlotPage;
		
		int startingIndex = incomingPickSlotPage * pickSlotPageSize;
		int endingIndex = startingIndex + pickSlotPageSize;

		for(int i = startingIndex; i < endingIndex; i++) {
			if(allPickTokens.Count == i) {      // Reached the end of list
				break;
			}

			// Try to see if token exists already in final list
			bool existsAlready = false;
			foreach(EpiPenGameSlot slot in finalSlotList) {
				EpiPenGameToken token = slot.GetToken();
                if(token != null && token.order == allPickTokens[i] && token.IsLocked == false) {
					existsAlready = true;
				}
			}

			// Only show it again if youre checking answer, or if it doesnt exist in finalSlot
			if(!existsAlready || isCheckingAnswer) {
				GameObject slotToken = GameObjectUtils.AddChildGUI(currentPickSlotList[i % pickSlotPageSize].gameObject, tokenPrefab);
				slotToken.GetComponent<EpiPenGameToken>().Init(allPickTokens[i], false);
			}
		}
		RefreshButtonShowStatus();
	}

	// Checks to see if the buttons need to appear
	private void RefreshButtonShowStatus() {
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
	#endregion
	#region Check tokens animations
	public void AnimateEnding(int card = 0) {
		// Hide the icon itself to fake it tweening
		finalSlotList[card].GetToken().GetComponent<Image>().enabled = false;

		// Spawn another panel that actually tweens
		GameObject panel = GameObjectUtils.AddChildGUI(animationTokenParent.gameObject, tokenPrefab);
		panel.transform.position = finalSlotList[card].GetComponent<RectTransform>().position;

		LeanTween.move(panel, animationTokenParent.position, 0.5f).setEase(LeanTweenType.easeInOutQuad);
		LeanTween.scale(panel, new Vector3(2f, 2f, 1f), 0.5f).setEase(LeanTweenType.easeInOutQuad);
		//playAnimation start coroutine after
		StartCoroutine(PlayAnimation(card, panel));
	}

	IEnumerator PlayAnimation(int card, GameObject panel) {
		yield return new WaitForSeconds(2.0f);
		LeanTween.move(panel, finalSlotList[card].transform.position, 0.5f).setEase(LeanTweenType.easeInOutQuad);
		LeanTween.scale(panel, Vector3.one, 0.5f).setEase(LeanTweenType.easeInOutQuad)
			.setOnComplete(delegate() { OnTokenAnimationDone(card, panel); });
	}

	private void OnTokenAnimationDone(int card, GameObject panel) {
		finalSlotList[card].GetToken().GetComponent<Image>().enabled = true;
		Destroy(panel.gameObject);
		card++;
		if(card < 8) { 
			AnimateEnding(card);
		}
		else {
			UIManager.ShowGameOver(attempts);
		}
	}
	#endregion
	#region Tutorial
	private void StartTutorial(int step = 0) {
		switch(step) {
			case 0:
				GameObjectUtils.AddChildGUI(currentPickSlotList[step].GetToken().gameObject, tutFingerPrefab);
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[0].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
			case 1:
				GameObjectUtils.AddChildGUI(currentPickSlotList[step].GetToken().gameObject, tutFingerPrefab);
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[5].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
			case 2:
				GameObjectUtils.AddChildGUI(currentPickSlotList[step].GetToken().gameObject, tutFingerPrefab);
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[3].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
			case 3:
				GameObjectUtils.AddChildGUI(currentPickSlotList[step].GetToken().gameObject, tutFingerPrefab);
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[7].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
		}
	}

	private void OnTutorialTweenComplete(int step) {
		Destroy(currentPickSlotList[step].GetToken().transform.GetChild(0).gameObject);
		switch(step) {
			case 0:
                finalSlotList[0].SetToken(currentPickSlotList[step].GetToken());
				break;
			case 1:
				finalSlotList[5].SetToken(currentPickSlotList[step].GetToken());
				break;
			case 2:
				finalSlotList[3].SetToken(currentPickSlotList[step].GetToken());
				break;
			case 3:
				finalSlotList[7].SetToken(currentPickSlotList[step].GetToken());
				break;
		}
		step++;
		if(step < 4) {
			StartTutorial(step);
		}
		else {
			StartCoroutine(EndTutorialCheckButton());
		}
	}

	IEnumerator EndTutorialCheckButton() {
		UIManager.CheckButtonToggle(true);
		yield return new WaitForSeconds(1.0f);
		isTutorial = false;
		DataManager.Instance.GameData.Tutorial.IsEpiPenGameTutorialDone = true;
		CheckAnswerButtonClicked();

		// Special case, circumvent new game here
		UIManager.StartGame();
    }
	#endregion
}