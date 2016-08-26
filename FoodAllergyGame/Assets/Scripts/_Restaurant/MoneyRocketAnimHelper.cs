using UnityEngine;

public class MoneyRocketAnimHelper : MonoBehaviour {
	DayOverMoneyRocketController rocketController;
	public void TriggerParticleEvent() {
		rocketController.PlayBooster();
    }
}
