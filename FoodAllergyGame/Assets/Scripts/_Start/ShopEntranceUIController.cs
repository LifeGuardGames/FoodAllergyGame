﻿using UnityEngine;

public class ShopEntranceUIController : MonoBehaviour {

	public GameObject tutorialFinger;
	public Animator animator;
	public GameObject glowSprite;

	void Start() {
		if(DataManager.Instance.GameData.Tutorial.IsSpeDecoTutDone) {
			bool isFirstTimeShop = DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance;
			StartManager.Instance.IsShopAppearHideDinerOverride = isFirstTimeShop;
			Show(isFirstTimeShop);
		}
		else {
			Hide();
		}
	}

	public void Show(bool isFirstTime){
		gameObject.SetActive(true);
		if(isFirstTime){
			ToggleClickable(true);
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
			PlayAppearAnimation();
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
	}

	public void AppearAnimationDoneEvent(){
		Debug.Log("CALL ME");
		// Show the tutorial finger
		tutorialFinger.SetActive(true);
		ToggleClickable(true); // NOTE: Don't show diner
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		glowSprite.SetActive(isClickable);
	}
}
