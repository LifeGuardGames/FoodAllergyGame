﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestaurantUIManager : MonoBehaviour{
	public DayOverUIController dayOverUIController;
	public Image clockBarFill;
	public GameObject clockFinishedImage;
	private bool isClockFinished;

	public void StartDay(){
		isClockFinished = false;
		clockFinishedImage.SetActive(false);
		clockBarFill.fillAmount = 0f;
	}

	public void UpdateClock(float totalTime, float timeLeft){
		if(!isClockFinished){
			float timeElapsed = totalTime - timeLeft;
			float percentage = timeElapsed / totalTime;
			clockBarFill.fillAmount = percentage;
		}
		else{
			Debug.LogWarning("Clock finished already but still updating");
		}
	}

	public void FinishClock(){
		clockFinishedImage.SetActive(true);
	}

	public void OnPauseButton(){
		RestaurantManager.Instance.PauseGame();
	}

	public void DayComplete(int customersMissed, float avgSatisfaction, int tips, int earningsNet, int totalCash, int medicCost){
		dayOverUIController.Populate(customersMissed, avgSatisfaction, tips, earningsNet, totalCash, medicCost);
		dayOverUIController.gameObject.SetActive(true);
		AudioManager.Instance.FadeOutPlayNewBackground(null);
		AudioManager.Instance.PlayClip("EndOfDay");
	}
}
