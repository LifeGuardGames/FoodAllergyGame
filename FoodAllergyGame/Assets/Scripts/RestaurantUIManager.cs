using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestaurantUIManager : MonoBehaviour {
	public Text cashText;

	public void UpdateCash(float cash){
		cashText.text = cash.ToString();
	}

	public void OnRestartButton(){
		RestaurantManager.Instance.RestartGame();
	}

	public void OnMedicButton(){
		RestaurantManager.Instance.DeployMedic();
	}
}
