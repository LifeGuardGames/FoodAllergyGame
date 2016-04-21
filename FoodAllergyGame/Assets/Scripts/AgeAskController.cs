using UnityEngine;
using UnityEngine.UI;

public class AgeAskController : MonoBehaviour {

	public TweenToggle panelTween;
	public InputField inputText;
	public Animation inputPulse;
	public GameObject button;

	public void ShowPanel() {
		panelTween.Show();
	}

	public void HidePanel() {
		panelTween.Hide();
	}

	public string GetAge() {
		return inputText.text;
	}

	public void ShowAgeButton() {
		button.SetActive(true);
	}

	// hide panel and then call start manager to clean up
	public void OnCollectAgeButton() {
		if(string.Equals(GetAge(), "Age") || string.IsNullOrEmpty(GetAge())){
			inputPulse.Play();
        }
		else {
			HidePanel();
			StartManager.Instance.CollectAge(GetAge());
		}
    }
}
