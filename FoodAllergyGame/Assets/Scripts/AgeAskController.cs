using UnityEngine;
using UnityEngine.UI;

public class AgeAskController : MonoBehaviour {

	public TweenToggle panelTween;
	public InputField inputText;
	public Animation inputPulse;

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
		if(string.Equals(GetAge(), "Age") || string.IsNullOrEmpty(GetAge())){
			inputPulse.Play();
        }
		else {
			StartManager.Instance.CollectAge(GetAge());
		}
    }
}
