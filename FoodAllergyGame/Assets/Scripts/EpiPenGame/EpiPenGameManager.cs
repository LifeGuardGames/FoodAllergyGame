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
				submittedAnswers.Remove(i);
				PlaceInPos(gamePanels[i]);
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

	public void PlaceInPos(EpiPenGamePanel panel) {
		foreach(Transform slot in pickSlots) {
			if(slot.childCount == 0) {
				panel.transform.SetParent(slot);
				//panel.transform.position = spot.position;
				panel.isCorrect = false;
				break;
			}
		}
	}
}
