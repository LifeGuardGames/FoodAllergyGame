using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayOverMoneyRocketController : MonoBehaviour {
	public Image fillImage;
	public TweenToggleDemux demux;

	private float fillHeight;	// Height of the sprite
	private int fillMax = 800;
	private int fillInterval = 100;
	private float fillIntervalPercentage;
	private int fillCashTarget;
	private int currentInterval;
	private float fillPitchIncrease;
	private float fillPitch;

	public void ShowRocket(){
		fillImage.fillAmount = 0;
		demux.Show();
	}

	public void OnShowComplete(){
		// Reset everything
		currentInterval = 0;
		fillHeight = fillImage.rectTransform.sizeDelta.y;
		fillIntervalPercentage = (float)fillInterval / fillMax;

		FillOneInterval();
	}

	private void FillOneInterval(){
		if(fillCashTarget > fillMax){
			fillCashTarget = fillMax;
		}
		if(currentInterval * fillInterval > fillCashTarget){
			return;
		}

		float startPercentage = (currentInterval * fillInterval) / (float)fillMax;
		float endPercentage = Mathf.Min(((currentInterval+1) * fillInterval), fillCashTarget) / (float)fillMax;
		LeanTween.value(gameObject, TweenHelper, startPercentage, endPercentage, 0.5f)
			.setEase(LeanTweenType.easeInOutQuad)
			.setDelay(0.2f);
		
		currentInterval++;
	}

	private void TweenHelper(float percentage){
		fillImage.fillAmount = percentage;
	}
}
