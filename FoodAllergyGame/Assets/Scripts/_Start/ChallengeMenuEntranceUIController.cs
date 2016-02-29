using UnityEngine;
using System.Collections;

public class ChallengeMenuEntranceUIController : MonoBehaviour {

	public Animator animator;
	public GameObject glowSprite;

	public void Show(bool isFirstTime) {
		gameObject.SetActive(true);
		if(isFirstTime) {
			ToggleClickable(false);
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
			PlayAppearAnimation();
		}
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	void OnMouseUpAsButton() {
		StartManager.Instance.ChallengeMenuButtonClicked();
	}

	//	void OnGUI(){
	//		if(GUI.Button(new Rect(100, 100, 100, 100), "play")){
	//			PlayAppearAnimation();
	//		}
	//	}

	private void PlayAppearAnimation() {
		
	}

	public void AppearAnimationStartEvent() {
	}

	public void AppearAnimationDoneEvent() {
		ToggleClickable(true); // NOTE: Don't show diner
	}

	public void ToggleClickable(bool isClickable) {
		GetComponent<BoxCollider2D>().enabled = isClickable;
		glowSprite.SetActive(isClickable);
	}
}
