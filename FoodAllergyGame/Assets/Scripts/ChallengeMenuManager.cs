using UnityEngine;
using System.Collections.Generic;

public class ChallengeMenuManager : Singleton<ChallengeMenuManager> {
	public GameObject challengeGrid;
	public GameObject challengeButtonPrefab;

	void Start() {
		List<ImmutableDataChallenge> challengeList = DataLoaderChallenge.GetDataList();
		foreach(ImmutableDataChallenge challengeData in challengeList) {
			GameObject challengeButton = GameObjectUtils.AddChildGUI(challengeGrid, challengeButtonPrefab);
			challengeButton.GetComponent<ChallengeButton>().Init(challengeData);
		}
	}

	public void StartChallenge(string challengeID) {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = challengeID;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT, showFoodTip: true);
	}
}
