﻿using UnityEngine.UI;
using UnityEngine;

public class StardustVendor : Singleton<StardustVendor> {
	public GameObject starDustHud;
	public TweenToggleDemux tween;

	public void BuyStardustSetOne() {
		PurchasingManager.Instance.BuyStardustSet(1);
		tween.Hide();
    }

	public void BuyStardustSetTwo() {
		PurchasingManager.Instance.BuyStardustSet(2);
		tween.Hide();
    }

	public void BuyStardustSetThree() {
		PurchasingManager.Instance.BuyStardustSet(3);
		tween.Hide();
    }

	public void UpdateStarHud() {
		starDustHud.gameObject.GetComponentInChildren<Text>().text = DataManager.Instance.GameData.DayTracker.IAPCurrency.ToString();
	}

	public void OnExitButton() {
		tween.Hide();
	}
}
