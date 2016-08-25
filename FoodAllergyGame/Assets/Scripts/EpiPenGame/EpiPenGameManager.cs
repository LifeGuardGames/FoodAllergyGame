using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{
	public int totalSteps = 8;
	public Canvas mainCanvas;
	public EpiPenGameUIManager UIManager;
	
	public List<EpiPenGameSlot> finalSlotList;
	private List<int> allPickTokens;

	public List<EpiPenGameSlot> currentPickSlotList;
	
	public Transform activeDragParent;
	public Sprite lockedFinalSlotSprite;
	public Sprite emptyFinalSlotSprite;

	public GameObject leftButton;
	public GameObject rightButton;
	
	public GameObject checkButton;
	public TweenToggle skipButtonTween;

	private int pickSlotPage = 0;
	private int pickSlotPageSize = 5;
	private List<bool> answers;
	public RectTransform animationTokenParent;
	public bool isTutorial;
	public GameObject tutFingerPrefab;
	public GameObject tutFingerPressPrefab;
	private int attempts = 0;
	private int difficulty = 0;
	private bool isAnimatingEnding = false;     // Used for skipping
	private bool isSkippingAnimations = false;	// Used for skipping

	private string epipenSetPrefix;     // "A" or "B", 'TokenA1' format

	void Start() {
		int randomIndex = UnityEngine.Random.Range(0, 2);
		epipenSetPrefix = randomIndex == 0 ? "A" : "B";

		if(DataManager.Instance.IsDebug) {
			isTutorial = !Constants.GetConstant<bool>("IsEpiPenTutorialDone");
		}
		else {
			if(DataManager.Instance.GameData.Tutorial.IsEpiPenGameTutorialDone) {
				isTutorial = false;
			}
			else {
				isTutorial = true;
				checkButton.GetComponent<Button>().enabled = false;
			}
		}
		StartGame();
		//skipButtonTween.Hide();
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

		int tokensToPick = DataManager.Instance.GameData.Epi.Difficulty;
		difficulty = tokensToPick;
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
				string suffixID = epipenSetPrefix + i;
                GameObject token = Resources.Load("Token" + suffixID) as GameObject;
				GameObject go = GameObjectUtils.AddChildGUI(finalSlotList[i].gameObject, token);
				finalSlotList[i].GetComponent<Image>().sprite = lockedFinalSlotSprite;
				go.GetComponent<EpiPenGameToken>().Init(i, true);
			}
		}

		StartCoroutine(ShowPage(0));
		if(isTutorial) {
			// Wait for show page to complete destroying and instantiating children
			yield return 0;
			yield return 0;
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
		AudioManager.Instance.PlayClip("EpipenCheck");
		UIManager.CheckButtonToggle(false);
		UIManager.OnCheckButton();

		// Check if all the tokens are filled
		foreach(EpiPenGameSlot slot in finalSlotList) {
			if(slot.GetToken() == null) {
				Debug.LogWarning("Final slots not filled");
				return;
			}
		}

		attempts++;
		AnimateEnding();
	}

	public void OnGameOverButtonClicked() {
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: true);
	}

	private bool IsTokenCorrect(int finalSlotIndex) {
		return finalSlotList[finalSlotIndex].GetToken().tokenNumber == finalSlotIndex;
    }

	private bool IsTokenIndexInFinalSlot(int index) {
		foreach(EpiPenGameSlot slot in finalSlotList) {
			EpiPenGameToken token = slot.GetToken();
            if(token != null) {
				if(index == token.tokenNumber) {
					return true;
				}
			}
		}
		return false;
	}

	#region Page button functions
	public void PageButtonClicked(bool isRightButton) {
		if(isRightButton) {
			pickSlotPage++;
		}
		else {
			pickSlotPage--;
		}
		StartCoroutine(ShowPage(pickSlotPage));
	}

	// Either refreshes current page, or shows a new page given page number
	private IEnumerator ShowPage(int _pickSlotPage) {
		// Destroy children beforehand
		foreach(EpiPenGameSlot slot in currentPickSlotList) {
			slot.ClearToken();
		}
		yield return 0;

		pickSlotPage = _pickSlotPage;
		
		int startingIndex = _pickSlotPage * pickSlotPageSize;
		int endingIndex = startingIndex + pickSlotPageSize;

		for(int i = startingIndex; i < endingIndex; i++) {
			if(allPickTokens.Count == i) {      // Reached the end of list
				break;
			}

			int currentPickTokenNumber = allPickTokens[i];
            if(!IsTokenIndexInFinalSlot(currentPickTokenNumber)) {  // TODO Needs to read from token list
				string suffixID = epipenSetPrefix + currentPickTokenNumber;
                GameObject token = Resources.Load("Token" + suffixID) as GameObject;
				GameObject slotToken = GameObjectUtils.AddChildGUI(currentPickSlotList[i % pickSlotPageSize].gameObject, token);
				slotToken.GetComponent<EpiPenGameToken>().Init(currentPickTokenNumber, false);
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
	public void OnSkipAnimationButton() {
		if(isAnimatingEnding) {
			// Fast check everything, but skipping the animations
			isSkippingAnimations = true;
        }
		else {
			Debug.LogError("Invalid skip animation call");
		}
		skipButtonTween.Hide();
    }

	public void AnimateEnding(int slotIndex = 0) {
		isAnimatingEnding = true;
		foreach (EpiPenGameSlot tok in finalSlotList) {
			tok.GetToken().GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		// Show the skip button on first call
		if(slotIndex == 0 && DataManager.Instance.GameData.Epi.hasSeenEnding) {
			skipButtonTween.Show();
		}

		// Grab token info and hide the token itself to fake it tweening
		EpiPenGameToken tokenInSlot = finalSlotList[slotIndex].GetToken();
		int tokenNumber = tokenInSlot.tokenNumber;
        tokenInSlot.gameObject.SetActive(false);

		// Spawn another panel that actually tweens
		GameObject tokenPrefab = Resources.Load("Token" + epipenSetPrefix + tokenNumber) as GameObject;
		GameObject animationTokenAux = GameObjectUtils.AddChildGUI(animationTokenParent.gameObject, tokenPrefab);
		animationTokenAux.GetComponent<EpiPenGameToken>().AuxInit();
		animationTokenAux.transform.position = finalSlotList[slotIndex].GetComponent<RectTransform>().position;
		animationTokenAux.GetComponent<CanvasGroup>().blocksRaycasts = false;
        float moveTime = isSkippingAnimations ? 0f : 0.5f;
        LeanTween.move(animationTokenAux, animationTokenParent.position, moveTime).setEase(LeanTweenType.easeInOutQuad);
		LeanTween.scale(animationTokenAux, new Vector3(2f, 2f, 1f), moveTime).setEase(LeanTweenType.easeInOutQuad)
			.setOnComplete(delegate () { StartCoroutine(PlayAnimation(slotIndex, animationTokenAux)); });

		UIManager.FadeToggle(true);
	}

	IEnumerator PlayAnimation(int slotIndex, GameObject animationTokenAux) {
		animationTokenAux.GetComponent<EpiPenGameToken>().SetAnimateState(true);
		yield return new WaitForSeconds(isSkippingAnimations ? 0f : 1.0f);

		// Show the symbol animation
		bool isCorrect = IsTokenCorrect(slotIndex);
		animationTokenAux.GetComponent<EpiPenGameToken>().SetMark(isCorrect, true);

		yield return new WaitForSeconds(isSkippingAnimations ? 0f : 0.5f);

		// Check to see if it is correct and do the appropriate action
		if(isCorrect) {
			finalSlotList[slotIndex].GetToken().IsLocked = true;		// Lock the token
            if(allPickTokens.Remove(slotIndex)) {						// Soft remove from pick list
				finalSlotList[slotIndex].GetComponent<Image>().sprite = lockedFinalSlotSprite;	// Change slot color
			}

			// Continue animation sequence
			float moveTime = isSkippingAnimations ? 0f : 0.5f;
			LeanTween.move(animationTokenAux, finalSlotList[slotIndex].transform.position, moveTime).setEase(LeanTweenType.easeInOutQuad);
			LeanTween.scale(animationTokenAux, Vector3.one, moveTime).setEase(LeanTweenType.easeInOutQuad)
				.setOnComplete(delegate () { OnTokenAnimationDone(slotIndex, animationTokenAux); });

			UIManager.FadeToggle(false);
		}
		else {
			skipButtonTween.Hide();
			isSkippingAnimations = false;
			UIManager.FadeToggle(false);
			Destroy(animationTokenAux.gameObject);

			// Hide all the marks if there are any
			foreach(EpiPenGameSlot slot in finalSlotList) {
				slot.GetToken().HideMark();
			}

			// Return all tokens that is not locked, this and after
			for(int i = slotIndex; i < totalSteps; i++) {
				EpiPenGameSlot finalSlot = finalSlotList[i];
				if(!finalSlot.GetToken().IsLocked) {
					if(!isTutorial) {
						AnalyticsManager.Instance.MissedPiece(finalSlotList[i].GetToken().tokenNumber);
					}
					finalSlot.ClearToken();
					finalSlot.GetComponent<Image>().sprite = emptyFinalSlotSprite;
				}
			}
			isTutorial = false;
			// Continue the timer since incorrect
			UIManager.ContinueTimer();

			isAnimatingEnding = false;
		}
		StartCoroutine(ShowPage(0));
	}
	
	private void OnTokenAnimationDone(int slotIndex, GameObject animationTokenAux) {
		finalSlotList[slotIndex].GetToken().gameObject.SetActive(true);
		Destroy(animationTokenAux.gameObject);
		slotIndex++;
		if(slotIndex < 8) { 
			AnimateEnding(slotIndex);
		}
		else {
			if(!DataManager.Instance.GameData.Epi.hasSeenEnding) {
				DataManager.Instance.GameData.Epi.hasSeenEnding = true;
            }
			isSkippingAnimations = false;
			isAnimatingEnding = false;
            UIManager.ShowGameOver(attempts);
			AnalyticsManager.Instance.EpiPenGameResultsAalytics(attempts, difficulty, UIManager.timerText.text);
		}
	}
	#endregion

	#region Tutorial
	private void StartTutorial(int step = 0) {
		GameObjectUtils.AddChildGUI(currentPickSlotList[step].GetToken().gameObject, tutFingerPrefab).name = "EpipenFinger";
		switch(step) {
			case 0:
				AudioManager.Instance.PlayClip("PickUp");
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[0].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
			case 1:
				AudioManager.Instance.PlayClip("PickUp");
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[5].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
			case 2:
				AudioManager.Instance.PlayClip("PickUp");
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[3].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
			case 3:
				AudioManager.Instance.PlayClip("PickUp");
				LeanTween.move(currentPickSlotList[step].GetToken().gameObject, finalSlotList[7].transform.position, 1.0f)
					.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(delegate () { OnTutorialTweenComplete(step); });
				break;
		}
	}

	private void OnTutorialTweenComplete(int step) {
		Destroy(currentPickSlotList[step].GetToken().transform.Find("EpipenFinger").gameObject);
		switch(step) {
			case 0:
				AudioManager.Instance.PlayClip("Drop");
				finalSlotList[0].SetToken(currentPickSlotList[step].GetToken());
				break;
			case 1:
				AudioManager.Instance.PlayClip("Drop");
				finalSlotList[5].SetToken(currentPickSlotList[step].GetToken());
				break;
			case 2:
				AudioManager.Instance.PlayClip("Drop");
				finalSlotList[3].SetToken(currentPickSlotList[step].GetToken());
				break;
			case 3:
				AudioManager.Instance.PlayClip("Drop");
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
		GameObject go = GameObjectUtils.AddChildGUI(checkButton, tutFingerPressPrefab);
		Destroy(go, 1.2f);
		yield return new WaitForSeconds(1.0f);
		DataManager.Instance.GameData.Tutorial.IsEpiPenGameTutorialDone = true;
		CheckAnswerButtonClicked();
		attempts = 0;
		UIManager.timer.ResetTimer();
		// Special case, circumvent new game here
		checkButton.GetComponent<Button>().enabled = true;
		UIManager.StartGame();
    }
	#endregion

	public float GetCanvasDistanceOffset(){
		return mainCanvas.planeDistance;
	}
}
