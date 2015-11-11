using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayOverUIController : MonoBehaviour {

	public Text textCustomersMissed;
	public Text textTips;
	public Text textMenuCost;
	public Text textEarningsNet;

	public void Populate(int customersMissed, float avgSatisfaction, int tips, int earningsNet, int totalCash, int medicalExpenses){
		textCustomersMissed.text = customersMissed.ToString();
		textTips.text = tips.ToString();
		textMenuCost.text = medicalExpenses.ToString();
		textEarningsNet.text = (tips + medicalExpenses).ToString();
	}
}
