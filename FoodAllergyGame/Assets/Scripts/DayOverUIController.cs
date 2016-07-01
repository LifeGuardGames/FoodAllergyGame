using UnityEngine;
using UnityEngine.UI;

public class DayOverUIController : MonoBehaviour {
	public TweenToggleDemux tweenDemux;
	public Text textCustomersMissed;
	public Text textTips;
	public Text textMenuCost;
	public Text textEarningsNet;
	public Text textBonus;
	public Image imgBonus;
	public ParticleSystem dayOverParticle;

	public void ShowPanel() {
		tweenDemux.Show();
		dayOverParticle.Play();
	}

	public void HidePanel() {
		tweenDemux.Hide();
	}

	public void Populate(int customersMissed, int tips, int medicalExpenses, int earningsNet) {
		textCustomersMissed.text = customersMissed.ToString();
		textTips.text = tips.ToString();
		textMenuCost.text = medicalExpenses.ToString();
		textEarningsNet.text = earningsNet.ToString();
		ImmutableDataBonusObjective temp  = DataLoaderBonusObjective.GetData(DataManager.Instance.GetBonus());
		switch(temp.ObjType) {
			case "Cash":
				textBonus.text = earningsNet.ToString() + " / " + temp.Num.ToString();
				break;
			case "AllergyAttack":
				textBonus.text = RestaurantManager.Instance.numOfAllergyAttacks.ToString() + " / " + temp.Num.ToString();
				break;
			case "Missed":
				textBonus.text = customersMissed.ToString() + " / " + temp.Num.ToString();
				break;
			case "Wheat":
				textBonus.text = RestaurantManager.Instance.wheatServed.ToString() + " / " + temp.Num.ToString();
				break;
			case "Dairy":
				textBonus.text = RestaurantManager.Instance.dairyServed.ToString() + " / " + temp.Num.ToString();
				break;
			case "Peanut":
				textBonus.text = RestaurantManager.Instance.peanutServed.ToString() + " / " + temp.Num.ToString();
				break;
			case "VIP":
				textBonus.text = RestaurantManager.Instance.VIPUses.ToString() + " / " + temp.Num.ToString();
				break;
			case "PlayArea":
				textBonus.text = RestaurantManager.Instance.PlayAreaUses.ToString() + " / " + temp.Num.ToString();
				break;
		}
		if(RestaurantManagerArcade.Instance.GetComponent<RestaurantManagerArcade>().checkBonus()) {

		}
	}
}
