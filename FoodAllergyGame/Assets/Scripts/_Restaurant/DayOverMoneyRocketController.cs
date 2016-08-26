using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DayOverMoneyRocketController : MonoBehaviour {
	public Image fillImage;
	public Animator rocketAnimator;
	public List<ParticleSystem> particleList;

	private float fillHeight;	// Height of the sprite
	private int fillMax = 800;
	private int fillCashTarget;
	private int fillIndexAux;
	private float fillPitchIncrease;
	private float fillPitch;
	private float endPercentage;

	public void ShowRocket(){
		fillImage.fillAmount = 0;
		fillIndexAux = 0;
	}

	public void OnShowComplete(){
		// Reset everything
		fillHeight = fillImage.rectTransform.sizeDelta.y;
		StartFill();
	}

	private void StartFill(){
		if(fillCashTarget > fillMax){
			fillCashTarget = fillMax;
		}
		float endPercentage = 0f;

		LeanTween.value(gameObject, TweenHelper, 0f, endPercentage, 1.5f)
			.setEase(LeanTweenType.easeInOutQuad)
			.setOnComplete(FillComplete);
	}

	private void TweenHelper(float percentage){
		fillImage.fillAmount = percentage;
		int interval = GetPercentageInterval(percentage);
		if(fillIndexAux != interval) {
			fillIndexAux = interval;
			rocketAnimator.SetTrigger("Pulse");
        }
    }

	private void FillComplete() {
		rocketAnimator.SetInteger("RocketPower", fillIndexAux);
    }

	public void PlayBooster() {
		particleList[fillIndexAux].gameObject.SetActive(true);
    }

	private int GetPercentageInterval(float percentage) {
		if(percentage < 0.2f) {
			return 0;
		}
		else if(percentage < 0.4f) {
			return 1;
		}
		else if(percentage < 0.6f) {
			return 2;
		}
		else if(percentage < 0.8f) {
			return 3;
		}
		else {
			return 4;
		}
	}
}
