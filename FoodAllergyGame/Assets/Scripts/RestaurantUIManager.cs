using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestaurantUIManager : MonoBehaviour {
	public Text cashText;
	public DayOverUIController dayOverUIController;

	public Image clockBarFill;
	public RectTransform clockHand;
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
			clockHand.transform.localEulerAngles = new Vector3(0, 0, -360f * percentage);
		}
		else{
			Debug.LogWarning("Clock finished already but still updating");
		}
	}

	public void FinishClock(){
		clockFinishedImage.SetActive(true);
	}

	public void UpdateCash(float cash){
		cashText.text = cash.ToString();
	}

	public void OnRestartButton(){
		RestaurantManager.Instance.RestartGame();
	}

	public void OnMedicButton(){
		RestaurantManager.Instance.DeployMedic();
	}

	public void DayComplete(int customersMissed, float avgSatisfaction, int tips, int menuCost, int earningsNet, int totalCash, int medicCost){
		dayOverUIController.Populate(customersMissed, avgSatisfaction, tips, menuCost, earningsNet, totalCash, medicCost);
		dayOverUIController.gameObject.SetActive(true);
//		AudioManager.Instance.PlayClip("EndOfDay");
	}
}
