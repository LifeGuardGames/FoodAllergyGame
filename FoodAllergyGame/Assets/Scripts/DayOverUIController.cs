using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayOverUIController : MonoBehaviour {

	public Text textCustomersMissed;
	public Text textAvgSatisfaction;
	public Text textTips;
	public Text textMenuCost;
	public Text textEarningsNet;
	public Text textTotalCash;

	public void Populate(int customersMissed, float avgSatisfaction, int tips, int menuCost, int earningsNet, int totalCash){
		textCustomersMissed.text = customersMissed.ToString();
		textAvgSatisfaction.text = avgSatisfaction.ToString("F2");	// Round to two decimal places
		textTips.text = tips.ToString();
		textMenuCost.text = menuCost.ToString();
		textEarningsNet.text = earningsNet.ToString();
		textTotalCash.text = totalCash.ToString();
	}
}
