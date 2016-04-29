using UnityEngine;
using UnityEngine.UI;

public class DayOverUIController : MonoBehaviour {
	public TweenToggleDemux tweenDemux;
	public Text textCustomersMissed;
	public Text textTips;
	public Text textMenuCost;
	public Text textEarningsNet;

	public void ShowPanel() {
		tweenDemux.Show();
	}

	public void HidePanel() {
		tweenDemux.Hide();
	}

	public void Populate(int customersMissed, int tips, int medicalExpenses, int earningsNet) {
		textCustomersMissed.text = customersMissed.ToString();
		textTips.text = tips.ToString();
		textMenuCost.text = medicalExpenses.ToString();
		textEarningsNet.text = earningsNet.ToString();
	}
}
