using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DayOverMoneyRocketController : MonoBehaviour {
	public Image fillImage;
	public Animator rocketAnimator;
	public List<ParticleSystem> particleList;
	public ParticleSystem particleSparkle;

	private int fillMax = 800;
	private int fillCashTarget;
	private int fillIndexAux;
	private float fillPitchIncrease = 0.12f;
	private float endPercentage;

	public void Init(int netEarning){
		fillCashTarget = netEarning;
        fillImage.fillAmount = 0;
		fillIndexAux = -1;
	}

	public void OnShowComplete(){
		// Reset everything
		StartFill();
	}

	private void StartFill(){
		if(fillCashTarget > fillMax){
			fillCashTarget = fillMax;
		}
		else if(fillCashTarget < 0) {
			fillCashTarget = 0;
        }
		float endPercentage = (float)fillCashTarget / fillMax;
		LeanTween.value(gameObject, TweenHelper, 0f, endPercentage, 2f)
			.setEase(LeanTweenType.easeOutQuad)
			.setOnComplete(FillComplete)
			.setDelay(0.5f);
	}

	private void TweenHelper(float percentage){
		fillImage.fillAmount = percentage;
		int interval = GetPercentageInterval(percentage);
		if(fillIndexAux != interval) {
			fillIndexAux = interval;
			rocketAnimator.SetTrigger("Pulse");
			Hashtable options = new Hashtable();
			options.Add("Pitch", 1f + (fillPitchIncrease * fillIndexAux));
            AudioManager.Instance.PlayClip("CoinGet1", option: options);

			if(fillIndexAux == 7) {
				particleSparkle.Play();
            }
        }
    }

	private void FillComplete() {
		StartCoroutine(FillComleteHelper());
    }

	private IEnumerator FillComleteHelper() {
		yield return new WaitForSeconds(0.8f);
		rocketAnimator.Play("MoneyRocketBlastStart");
	}

	public void PlayLaunchSound() {
		if(fillIndexAux < 2) {
			AudioManager.Instance.PlayClip("RocketLaunchFart");
		}
		else {
			AudioManager.Instance.PlayClip("RocketLaunch");
		}
	}

	public void PlayBooster() {
		particleList[fillIndexAux/2].gameObject.SetActive(true);
    }

	private int GetPercentageInterval(float percentage) {
		if(percentage < 0.125f) {
			return 0;
		}
		else if(percentage < 0.25f) {
			return 1;
		}
		else if(percentage < 0.375f) {
			return 2;
		}
		else if(percentage < 0.5f) {
			return 3;
		}
		else if(percentage < 0.625f) {
			return 4;
		}
		else if(percentage < 0.75f) {
			return 5;
		}
		else if(percentage < 0.875f) {
			return 6;
		}
		else {
			return 7;
		}
	}
}
