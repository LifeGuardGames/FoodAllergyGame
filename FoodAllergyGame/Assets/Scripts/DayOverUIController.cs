using UnityEngine;
using UnityEngine.UI;

public class DayOverUIController : MonoBehaviour {
	public DayOverMoneyRocketController rocketController;
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
		rocketController.Init(earningsNet);
		if(RestaurantManagerArcade.Instance.GetComponent<RestaurantManagerArcade>() != null) {
			
			ImmutableDataBonusObjective bonusData = DataLoaderBonusObjective.GetData(DataManager.Instance.GetBonus());
			textBonus.enabled = true;
			switch(bonusData.ObjType) {
				case "Cash":
					textBonus.text = tips.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "AllergyAttack":
					textBonus.text = RestaurantManager.Instance.numOfAllergyAttacks.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "Missed":
					textBonus.text = customersMissed.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "Wheat":
					textBonus.text = RestaurantManager.Instance.wheatServed.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "Dairy":
					textBonus.text = RestaurantManager.Instance.dairyServed.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "Peanut":
					textBonus.text = RestaurantManager.Instance.peanutServed.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "VIP":
					textBonus.text = RestaurantManager.Instance.VIPUses.ToString() + " / " + bonusData.Num.ToString();
					break;
				case "PlayArea":
					textBonus.text = RestaurantManager.Instance.PlayAreaUses.ToString() + " / " + bonusData.Num.ToString();
					break;
			}
			if(RestaurantManagerArcade.Instance.GetComponent<RestaurantManagerArcade>().checkBonus()) {
				imgBonus.enabled = true;
            }
		}
	}
}
