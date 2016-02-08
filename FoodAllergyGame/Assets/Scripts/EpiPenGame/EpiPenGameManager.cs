using UnityEngine;
using System.Collections.Generic;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{
	private List<bool> answers;
	public List<Transform> pickSlots;
	public List<EpiPenGamePanel> gamePanels;
	public EpiPenGameUIManager UIManager;
	public Dictionary<int, int> submittedAnswers;

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
		for(int i = 0; i < gamePanels.Count; i++) {
			answers.Add(true);
		}
		for(int i = 0; i < gamePanels.Count; i++) {
			submittedAnswers.Add(i,i);
		}
		RemovePieces();
		SetUpScene();
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
	/// set up scene marks correct answers as locked so the player can not move them and removes incorrect answers based on the answer list
	/// </summary>
	public void SetUpScene() {
		for(int i = 0; i < answers.Count; i++) {
			if(answers[i]) {
				gamePanels[i].Locked();
			}
			else {
				submittedAnswers.Remove(i);
				PlaceInPos(gamePanels[i]);
			}
		}
	}
	/// <summary>
	/// Only called once 
	/// initial set up to remove a number of panels for the player to put back
	/// the number is equal to half their current tier
	/// to remove a piece we mark them as false in answers so that setUpScene will remove them from the slot and we remove it from the submitted anwsers
	/// </summary>
	public void RemovePieces() {
		//int diff = TierManager.Instance.CurrentTier / 2;
		int diff = 3;
		List<EpiPenGamePanel> temp = new List<EpiPenGamePanel>();
		for(int i = 0; i < gamePanels.Count; i++) {
			temp.Add(gamePanels[i]);
		}
		for(int i = 0; i < diff; i++) {
			int rand = Random.Range(0, temp.Count-1);
			temp.RemoveAt(rand);
			answers[rand] = false;
			submittedAnswers.Remove(rand);
		}
	}
	/// <summary>
	/// Simply moves the in play pieces to a selection area and marks it as incorrect
	/// </summary>
	/// <param name="panel"></param>
	public void PlaceInPos(EpiPenGamePanel panel) {
		foreach(Transform slot in pickSlots) {
			if(slot.childCount == 0) {
				panel.transform.SetParent(slot);

				panel.isCorrect = false;
				break;
			}
		}
	}
}
