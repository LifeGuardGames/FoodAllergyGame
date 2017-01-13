using UnityEngine.UI;
using UnityEngine;

public class StardustVendor : MonoBehaviour {
	public GameObject starDustHud;
	public TweenToggleDemux tween;

	public void BuyStardustSetOne() {
		PurchasingManager.Instance.BuyConsumable(1);
		tween.Hide();
		UpdateStarHud();
    }

	public void BuyStardustSetTwo() {
		PurchasingManager.Instance.BuyConsumable(2);
		tween.Hide();
		UpdateStarHud();
    }

	public void BuyStardustSetThree() {
		PurchasingManager.Instance.BuyConsumable(3);
		tween.Hide();
		UpdateStarHud();
    }

	public void UpdateStarHud() {
		starDustHud.gameObject.GetComponentInChildren<Text>().text = DataManager.Instance.GameData.DayTracker.IAPCurrency.ToString();
	}
}
