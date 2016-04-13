﻿using UnityEngine;
using System.Collections;

public class RestaurantManagerLoader : MonoBehaviour {
	public GameObject RestArcade;
	public GameObject RestChallenge;

	void Start() {
		if(!string.IsNullOrEmpty(DataManager.Instance.GetChallenge())) {
			//Debug.Log(DataManager.Instance.GetChallenge());
			RestArcade.SetActive(false);
			RestaurantManagerChallenge.Instance.StartPhase();
		}
		else {
			RestChallenge.SetActive(false);
			RestaurantManagerArcade.Instance.StartPhase();
		}
	}
	
	// Called from PauseUIController
	public void QuitGame() {
		Time.timeScale = 1.0f;  // Remember to reset timescale!
		if(RestArcade.activeSelf) {
			int rand = Random.Range(1, 10);
			Debug.Log("Rand: " + rand);
			Debug.Log("Chance: " + DataManager.Instance.GameData.Epi.ChanceOfEpiGame);
			if(rand < DataManager.Instance.GameData.Epi.ChanceOfEpiGame) {
				DataManager.Instance.GameData.Epi.HasPlayedEpiPenGameThisTier = true;
				LoadLevelManager.Instance.StartLoadTransition(SceneUtils.EPIPEN, additionalImageKey: "LoadingImageEpipen");
			}
			else {
				if(!DataManager.Instance.GameData.Epi.HasPlayedEpiPenGameThisTier) {
					//DataManager.Instance.GameData.Epi.ChanceOfEpiGame += 10;
				}
				LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
			}
		}
		else {
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
		}
	}
	// Called from PauseUIController
	public void QuitGameChallenge() {
		Time.timeScale = 1.0f;  // Remember to reset timescale!
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.CHALLENGEMENU);
	}

	public void SkipTutorial() {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
		DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
	}
}