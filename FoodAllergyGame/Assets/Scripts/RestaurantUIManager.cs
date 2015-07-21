using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestaurantUIManager : MonoBehaviour {
	public Text cashText;
	public GameObject dayOverUI;
	public Image dayProgressBar;

	public void UpdateProgressBar(float totalTime, float timeLeft){
		float timeElapsed = totalTime - timeLeft;
		float percentage = timeElapsed / totalTime;
		dayProgressBar.fillAmount = percentage;
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

	public void DayComplete(float dayCash, int missingCustomers, float averageSatisfaction){
		dayOverUI.SetActive(true);
		dayOverUI.transform.GetChild(1).GetComponent<Text>().text += dayCash.ToString();
		dayOverUI.transform.GetChild(2).GetComponent<Text>().text += missingCustomers.ToString();
		dayOverUI.transform.GetChild(3).GetComponent<Text>().text += averageSatisfaction.ToString();
		AudioManager.Instance.PlayClip("EndOfDay");
	}
}
