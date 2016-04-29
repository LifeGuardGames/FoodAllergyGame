using UnityEngine;
using UnityEngine.UI;

public class DayOverUIController : MonoBehaviour {
	public TweenToggleDemux tweenDemux;
	public Text textCustomersMissed;
	public Text textTips;
	public Text textMenuCost;
	public Text textEarningsNet;
	public CanvasGroup canvasGroup;

	public void ShowPanel() {
		tweenDemux.Show();
		canvasGroup.blocksRaycasts = true;
	}

	public void HidePanel() {
		tweenDemux.Hide();
		canvasGroup.blocksRaycasts = false;
	}

	public void Populate(int customersMissed, int tips, int medicalExpenses, int earningsNet) {
		textCustomersMissed.text = customersMissed.ToString();
		textTips.text = tips.ToString();
		textMenuCost.text = medicalExpenses.ToString();
		textEarningsNet.text = earningsNet.ToString();
	}
}
