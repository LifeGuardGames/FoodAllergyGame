using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StardustVendor : MonoBehaviour {
	public GameObject starDustHud;

	public void BuyOneStardust() {
		PurchasingManager.Instance.BuyConsumable(1);
		this.GetComponent<PositionTweenToggle>().Hide();
		UpdateStarHud();
    }

	public void BuyTwoStardust() {
		PurchasingManager.Instance.BuyConsumable(2);
		this.GetComponent<PositionTweenToggle>().Hide();
		UpdateStarHud();
    }

	public void BuyThreeStardust() {
		PurchasingManager.Instance.BuyConsumable(3);
		this.GetComponent<PositionTweenToggle>().Hide();
		UpdateStarHud();
    }

	public void UpdateStarHud() {
		starDustHud.gameObject.GetComponentInChildren<Text>().text = DataManager.Instance.GameData.DayTracker.IAPCurrency.ToString();
	}
}
