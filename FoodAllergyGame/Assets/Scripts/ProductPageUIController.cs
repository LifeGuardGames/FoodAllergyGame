using UnityEngine;

public class ProductPageUIController : MonoBehaviour {
	public TweenToggleDemux demux;
	public GameObject redeemButton;

	void Start() {
		redeemButton.SetActive(false);
	}

	public void ShowPanel() {
		demux.Show();
    }

	public void OnShowComplete() {
#if UNITY_IOS
		redeemButton.SetActive(true);
#endif
	}

	public void HidePanel() {
		demux.Hide();
<<<<<<< Updated upstream
		//canvasGroup.blocksRaycasts = false;
=======
>>>>>>> Stashed changes
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
