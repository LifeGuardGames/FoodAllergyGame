﻿using UnityEngine;

public class RestaurantUIManager : MonoBehaviour{
	public DoorController doorController;
	public DayOverUIController dayOverUIController;
	public ChallengeOverUi challengeOverUiController;
	private bool isClockFinished;

	public void StartDay(){
		isClockFinished = false;
		doorController.ResetDayAlpha();
    }

	public void UpdateClock(float totalTime, float timeLeft){
		if(!isClockFinished){
			float timeElapsed = totalTime - timeLeft;
			float percentage = timeElapsed / totalTime;
			doorController.DayAlphaTweenUpdate(percentage);
		}
		else{
			Debug.LogWarning("Clock finished already but still updating");
		}
	}

	public void FinishClock(){
		doorController.LockdownDoor();
    }

	public void OnPauseButton(){
		RestaurantManager.Instance.PauseGame();
	}

	public void DayComplete(int customersMissed, int tips, int medicCost, int earningsNet){
		dayOverUIController.Populate(customersMissed, tips, medicCost, earningsNet);
		dayOverUIController.gameObject.SetActive(true);
		AudioManager.Instance.FadeOutPlayNewBackground(null);
		AudioManager.Instance.PlayClip("EndOfDay");
	}

	public void ChallengeComplete(int score, int cashEarned, int negativeCash) {
		challengeOverUiController.Populate(negativeCash, cashEarned, score);
		challengeOverUiController.gameObject.SetActive(true);
		challengeOverUiController.StartBar();
		AudioManager.Instance.FadeOutPlayNewBackground(null);
		AudioManager.Instance.PlayClip("EndOfDay");
	}

	public void UpdateCashUI(Vector3 customerPosition, int billAmount) {
		ParticleUtils.PlayMoneyFloaty(customerPosition, billAmount);
		
		// Play different sounds depending on how much money you get
		if(billAmount > 0) {
			if(billAmount < 5) {
				AudioManager.Instance.PlayClip("CoinGet1");
			}
			else if(billAmount < 10) {
				AudioManager.Instance.PlayClip("CoinGet2");
			}
			else {
				AudioManager.Instance.PlayClip("CoinGet3");
			}
		}
		else if(billAmount < 0) {
			AudioManager.Instance.PlayClip("CoinLose");
		}
	}

	public void OpenAndCloseDoor() {
		doorController.OpenAndCloseDoor();
	}

	public void ResetDoor() {
		doorController.ResetDoor();
	}
}
