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
		//int diff = TierManager.Instance.CurrentTier / 2;
		int diff = 3;
		List<EpiPenGameToken> temp = new List<EpiPenGameToken>();
		for(int i = 0; i < epiPenTokens.Count; i++) {
			temp.Add(epiPenTokens[i]);
		}
//		Debug.Log(EpiPenGameManager.Instance.submittedAnswers.Count);

		foreach(int randomIndex in NumberUtils.UniqueRandomList(3, 0, totalSteps - 1)) {

		}
		for(int i = 0; i < diff; i++) {
			int rand = Random.Range(0, temp.Count - 1);
			int auxIndex = temp[rand].order;	// Leverage list for removal, but still keep value
            temp.RemoveAt(rand);
			submittedAnswers.Remove(auxIndex);
			answers[auxIndex] = false;
		}
//		Debug.Log(EpiPenGameManager.Instance.submittedAnswers.Count);
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
}
