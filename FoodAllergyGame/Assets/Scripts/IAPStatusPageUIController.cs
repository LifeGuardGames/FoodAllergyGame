using UnityEngine;
using UnityEngine.UI;

public class IAPStatusPageUIController : MonoBehaviour {
	public Text description;
	public TweenToggleDemux panelTween;

	public void ShowPanel(bool isSuccess) {
		description.text = LocalizationText.GetText(isSuccess ? "IAPProSuccess" : "IAPProFailure");
		StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(false);
		StartManager.Instance.DinerEntranceUIController.ToggleClickable(false);
		StartManager.Instance.ShopEntranceUIController.ToggleClickable(false);
		panelTween.Show();
	}

	public void OnOkButtonClicked() {
		StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
		StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
		StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
		panelTween.Hide();
	}
}
