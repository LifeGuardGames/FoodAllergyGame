using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{

	private List<bool> answers;
	public List<EpiPenGamePanel> gamePanels;
	public EpiPenGameUiManager uiManager;
	public Dictionary<int, int> submittedAnswers;

	void Start() {
		StartGame();
	}

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
		}
		else {
			SetUpScene();
		}
	}

	public bool CheckForGameOver() {
		foreach(bool ans in answers) {
			if(!ans) {
				return false;
			}
		}
		return true;
	}

	public void SetUpScene() {
		for(int i = 0; i < answers.Count; i++) {
			if(answers[i]) {
				gamePanels[i].Locked();
			}
			else {
				uiManager.PlaceInPos(gamePanels[i]);
			}
		}
	}

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
}
