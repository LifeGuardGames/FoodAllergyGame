using UnityEngine;
using UnityEngine.UI;

public class ProductPageUIController : MonoBehaviour {
	public TweenToggleDemux demux;
	public GameObject redeemButton;
	public GameObject buyButton;
	public Text buttonText;

	void Start() {
		redeemButton.SetActive(false);
	}

	public void ShowPanel() {
		buttonText.text = "Get MORE\n" + DataManager.Instance.PriceStringAux;
        buyButton.SetActive(true);
        demux.Show();

#if UNITY_IOS
		redeemButton.SetActive(true);
#endif
    }

	public void HidePanel() {
		demux.Hide();
	}

	public void OnExitButton() {
		StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
		StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
		StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
		HidePanel();
	}

	public void OnBuyButton() {
		buyButton.SetActive(false);
		PurchasingManager.Instance.BuyPro();
	}

	// Should only be called on iOS
	public void OnRedeemButton() {
#if UNITY_IOS
		PurchasingManager.Instance.RestorePurchases();
		redeemButton.SetActive(false);
#else
		Debug.LogError("Button should not be active");
#endif
	}
}
