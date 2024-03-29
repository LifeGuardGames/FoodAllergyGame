﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeProgressBarController : MonoBehaviour{
	public ChallengeOverUIController challengeOverUIController;
	public RectTransform barParent;
	public RectTransform bronzeBar;
	public RectTransform silverBar;
	public RectTransform goldBar;

	private float parentBarWidth;
	private float bronzeBarFillAux;
	private float silverBarFillAux;
	private float goldBarFillAux;

	void Start(){
		parentBarWidth = barParent.rect.width;

		bronzeBarFillAux = 0;
		silverBarFillAux = 0;
		goldBarFillAux = 0;
	}

	public void DebugButton(){
		MoveBar();
	}

	public void MoveBar(){
		int score = RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().GetScore();
		int bronzeBreakPoint = RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.BronzeBreakPoint;
		int silverBreakPoint = RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.SilverBreakPoint;
		int goldBreakPoint = RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().chall.GoldBreakPoint;

		if(bronzeBarFillAux < 1.0f){
			if(score >= bronzeBreakPoint){
				LeanTween.value(this.gameObject, UpdateBronzeBar, 0f, 1f, 2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(OnBronzeBarComplete);
			}
			else{
				LeanTween.value(this.gameObject, UpdateBronzeBar, 0f, (float)score / (float)bronzeBreakPoint, 2f).setEase(LeanTweenType.easeInOutQuad);
			}
		}
		else if(silverBarFillAux < 1.0f){
			if(score >= silverBreakPoint){ 
				LeanTween.value(this.gameObject, UpdateSilverBar, 0f, 1f, 2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(OnSilverBarComplete);
			}
			else{
				LeanTween.value(this.gameObject, UpdateSilverBar, 0f, (float)score / (float)silverBreakPoint, 2f).setEase(LeanTweenType.easeInOutQuad);
			}
		}
		else if(goldBarFillAux < 1.0f){
			if(score >= goldBreakPoint){
				LeanTween.value(this.gameObject, UpdateGoldBar, 0f, 1f, 2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(OnGoldBarComplete);
			}
			else{
				LeanTween.value(this.gameObject, UpdateGoldBar, 0f, (float)score / (float)goldBreakPoint, 2f).setEase(LeanTweenType.easeInOutQuad);
			}
		}
	}

	private void UpdateBronzeBar(float val){
		bronzeBarFillAux = val;
		bronzeBar.offsetMax = new Vector2(-parentBarWidth * (1f - val), 0f);
	}

	private void OnBronzeBarComplete(){
		if(bronzeBarFillAux == 1.0f){
			challengeOverUIController.UpdateTrophy(ChallengeReward.Bronze);
			AudioManager.Instance.PlayClip("TrophyBronzeGet");
			MoveBar();
		}
	}

	private void UpdateSilverBar(float val){
		silverBarFillAux = val;
		silverBar.offsetMax = new Vector2(-parentBarWidth * (1f - val), 0f);
	}

	private void OnSilverBarComplete(){
		if(silverBarFillAux == 1.0f){
			challengeOverUIController.UpdateTrophy(ChallengeReward.Silver);
			AudioManager.Instance.PlayClip("TrophySilverGet");
			MoveBar();
		}
	}

	private void UpdateGoldBar(float val){
		goldBarFillAux = val;
		goldBar.offsetMax = new Vector2(-parentBarWidth * (1f - val), 0f);
	}

	private void OnGoldBarComplete(){
		if(goldBarFillAux == 1.0f){
			challengeOverUIController.UpdateTrophy(ChallengeReward.Gold);
			AudioManager.Instance.PlayClip("TrophyGoldGet");
		}
	}
}
