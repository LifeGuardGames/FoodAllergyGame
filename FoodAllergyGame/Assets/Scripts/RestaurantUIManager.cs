using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestaurantUIManager : MonoBehaviour {
	public Text cashText;
	public GameObject unlocked;
	public Image dayProgressBar;
	public DayOverUIController dayOverUIController;

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

	public void DayComplete(int customersMissed, float avgSatisfaction, int tips, int menuCost, int earningsNet, int totalCash){
		if(DataManager.Instance.GetEvent() == "Event0T"){	// TODO abstract out
			unlocked.SetActive(true);
		}

		dayOverUIController.Populate(customersMissed, avgSatisfaction, tips, menuCost, earningsNet, totalCash);
		dayOverUIController.gameObject.SetActive(true);
		AudioManager.Instance.PlayClip("EndOfDay");
	}
}
