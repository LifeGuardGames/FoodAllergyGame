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
	public Text textMedicCost;
	public Text textTotalExpense;

	public void Populate(int customersMissed, float avgSatisfaction, int tips, int earningsNet, int totalCash, int medicalExpenses){
		textCustomersMissed.text = customersMissed.ToString();
		textAvgSatisfaction.text = avgSatisfaction.ToString("F2");	// Round to two decimal places
		textTips.text = tips.ToString();
		textMenuCost.text = medicalExpenses.ToString();
		textEarningsNet.text = (tips + medicalExpenses).ToString();
		textTotalCash.text = totalCash.ToString();
		textMedicCost.text = medicalExpenses.ToString();
		textTotalExpense.text = medicalExpenses.ToString();
	}


}
