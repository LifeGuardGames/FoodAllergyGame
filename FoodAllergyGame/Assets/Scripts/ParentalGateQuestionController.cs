using UnityEngine;
using UnityEngine.UI;

public class ParentalGateQuestionController : MonoBehaviour {
	public TweenToggleDemux panelTween;
	public InputField inputText;
	public Animation inputPulse;
	public GameObject button;

	public void ShowPanel() {
		button.SetActive(false);
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
	public void OnAnswerButton() {
		if(string.Equals(GetAge(), "Answer") || string.IsNullOrEmpty(GetAge())) {
			HidePanel();
			inputText.text = "Answer";
			StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
			StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
		}
		else if (string.Equals(GetAge(), "25")){
			HidePanel();
			inputText.text = "Answer";
			// TODO Uncomment this, feature removed from game
			//StartManager.Instance.ShowProductPage();		
		}
		else {
			inputText.text = "Answer";
			StartManager.Instance.ShopEntranceUIController.ToggleClickable(true);
			StartManager.Instance.DinerEntranceUIController.ToggleClickable(true);
			StartManager.Instance.ChallengeMenuEntranceUIController.ToggleClickable(true);
			HidePanel();
		}
		
	}
}
