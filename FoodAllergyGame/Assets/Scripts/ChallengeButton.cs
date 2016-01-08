using UnityEngine;
using System.Collections;

public class ChallengeButton : MonoBehaviour {
	public string challengeID;

	public void StartChallenge() {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = challengeID;
		Application.LoadLevel(SceneUtils.RESTAURANT);
	}
}
