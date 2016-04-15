using UnityEngine;
using UnityEngine.UI;

public class IAPStatusPageUIController : MonoBehaviour {
	public Text description;
	public TweenToggleDemux panelTween;

	public void ShowPanel(bool isSuccess) {
		description.text = LocalizationText.GetText(isSuccess ? "IAPProSuccess" : "IAPProFailure");
		panelTween.Show();
	}

	public void OnOkButtonClicked() {
		panelTween.Hide();
	}
}
