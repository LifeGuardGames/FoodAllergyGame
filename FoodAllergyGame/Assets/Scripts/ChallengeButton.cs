using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChallengeButton : MonoBehaviour {
	public string challengeID;

	public void StartChallenge() {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = challengeID;
		SceneManager.LoadScene(SceneUtils.RESTAURANT);
	}
}
