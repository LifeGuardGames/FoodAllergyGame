using UnityEngine;
using System.Collections;

public class DecoEntranceUIController : MonoBehaviour {

	public GameObject tutorialFinger;

	public void Show(bool isFirstTime){
		gameObject.SetActive(true);
		if(isFirstTime && !DataManager.Instance.IsDebug){
			// Show the tutorial finger
			tutorialFinger.SetActive(true);
		}
		else{
			tutorialFinger.SetActive(false);
		}
	}

	public void Hide(){
		gameObject.SetActive(false);
	}

	void OnMouseUpAsButton(){
		StartManager.Instance.DecoButtonClicked();
	}
}
