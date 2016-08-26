using UnityEngine;

public class MoneyRocketAnimHelper : MonoBehaviour {
	public DayOverMoneyRocketController rocketController;

	public void TriggerParticleEvent() {
		rocketController.PlayBooster();
    }

	public void PlayLaunchSound() {
		rocketController.PlayLaunchSound();
	}
}
