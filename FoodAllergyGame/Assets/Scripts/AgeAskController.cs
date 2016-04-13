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

	public void OnCollectAgeButton() {
		HidePanel();
		StartManager.Instance.CollectAge(GetAge());
    }
}
