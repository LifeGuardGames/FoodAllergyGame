using UnityEngine;

public class ProductPageUIController : MonoBehaviour {
	public TweenToggleDemux demux;
	public GameObject redeemButton;
	public CanvasGroup canvasGroup;

	void Start() {
		redeemButton.SetActive(false);
	}

	public void ShowPanel() {
		demux.Show();
		canvasGroup.blocksRaycasts = true;
    }

	public void OnShowComplete() {
#if UNITY_IOS
		redeemButton.SetActive(true);
#endif
	}

	public void HidePanel() {
		demux.Hide();
		canvasGroup.blocksRaycasts = false;
	}

	public void OnExitButton() {
		HidePanel();
	}

	public void OnBuyButton() {
		PurchasingManager.Instance.BuyPro();
	}

	// Should only be called on iOS
	public void OnRedeemButton() {
#if UNITY_IOS
		PurchasingManager.Instance.RestorePurchases();
#else
		Debug.LogError("Button should not be active");
#endif
	}
}
