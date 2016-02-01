using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EpiPenGameManager : Singleton<EpiPenGameManager>{

	private List<bool> answers;
	public List<EpiPenGamePanel> gamePanels;
	public EpiPenGameUiManager uiManager;

	public void StartGame() {
		RemovePieces();
		SetUpScene();
	}

	public void CheckAnswers(Dictionary<int,int> submittedAnswers) {
		for(int i = 0; i < submittedAnswers.Count; i++) {
			if(submittedAnswers[i] == i) {
				answers.Add(true);
			}
			else {
				answers.Add(false);
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
		int diff = TierManager.Instance.CurrentTier / 2;
		List<EpiPenGamePanel> temp = gamePanels;
		for(int i = 0; i < diff; i++) {
			int rand = Random.Range(0, temp.Count);
			temp.RemoveAt(rand);
			answers[rand] = false;
		}
	}
}
