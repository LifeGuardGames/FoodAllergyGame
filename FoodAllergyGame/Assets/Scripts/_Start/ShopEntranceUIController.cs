using UnityEngine;
using System.Collections;

public class ShopEntranceUIController : MonoBehaviour {

	public GameObject tutorialFinger;
	public Animator animator;
	public GameObject glowSprite;

	public void Show(bool isFirstTime){
		gameObject.SetActive(true);
		if(isFirstTime){
			PlayAppearAnimation();
			StartManager.Instance.DecoEntranceUIController.ToggleClickable(false);
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
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
	
//	void OnGUI(){
//		if(GUI.Button(new Rect(100, 100, 100, 100), "play")){
//			PlayAppearAnimation();
//		}
//	}

	private void PlayAppearAnimation(){
		animator.Play("ShopAppear");
	}

	public void AppearAnimationStartEvent() {
		StartManager.Instance.DecoEntranceUIController.ToggleClickable(false);
		StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
	}

	public void AppearAnimationDoneEvent(){
		// Show the tutorial finger
		tutorialFinger.SetActive(true);
		StartManager.Instance.DecoEntranceUIController.ToggleClickable(true);
		StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		glowSprite.SetActive(isClickable);
	}
}
