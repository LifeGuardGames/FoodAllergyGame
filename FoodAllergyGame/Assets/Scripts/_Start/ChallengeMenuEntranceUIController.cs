﻿using UnityEngine;
using System.Collections;

public class ChallengeMenuEntranceUIController : MonoBehaviour {

	public Animator animator;
	public GameObject glowSprite;

	void Start() {
		// Show the challenge menu entrance
		if(TierManager.Instance.CurrentTier >= 3) {
			bool isFirstTimeChallenge = DataManager.Instance.GameData.Challenge.IsFirstTimeChallengeEntrance;
			Show(isFirstTimeChallenge);
		}
		else {
			Hide();
		}
	}

	public void Show(bool isFirstTime) {
		gameObject.SetActive(true);
		if(isFirstTime) {
			ToggleClickable(true);
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
