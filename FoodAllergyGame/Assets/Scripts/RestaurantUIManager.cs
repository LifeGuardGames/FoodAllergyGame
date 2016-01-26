using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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

	public void DayComplete(int customersMissed, int tips, int medicCost, int earningsNet){
		dayOverUIController.Populate(customersMissed, tips, medicCost, earningsNet);
		dayOverUIController.gameObject.SetActive(true);
		AudioManager.Instance.FadeOutPlayNewBackground(null);
		AudioManager.Instance.PlayClip("EndOfDay");
	}

	public void UpdateCashUI(Vector3 customerPosition, int billAmount) {
		ParticleUtils.PlayMoneyFloaty(customerPosition, billAmount);

		Debug.Log("BILL " + billAmount);
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
}
