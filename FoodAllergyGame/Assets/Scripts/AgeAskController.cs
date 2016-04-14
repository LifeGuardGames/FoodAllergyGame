using UnityEngine;
using UnityEngine.UI;

public class AgeAskController : MonoBehaviour {

	public TweenToggle panelTween;
	public InputField inputText;

	public void ShowPanel() {
		panelTween.Show();
	}

	public void HidePanel() {
		panelTween.Hide();
	}

	public string GetAge() {
		return inputText.text;
	}
	// hide panel and then call start manager to clean up
	public void OnCollectAgeButton() {
		HidePanel();
		StartManager.Instance.CollectAge(GetAge());
    }
}
