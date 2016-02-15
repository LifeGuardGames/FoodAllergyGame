using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ChallengeMenuManager : Singleton<ChallengeMenuManager> {
	public GridLayoutGroup challengeGrid;
	public GameObject challengeButtonPrefab;

	void Start() {
		List<ImmutableDataChallenge> unorderedList = DataLoaderChallenge.GetDataList();
        List<ImmutableDataChallenge> challengeList = (from element in unorderedList
													  orderby element.ID ascending
													  select element).ToList();

		int regularChallengeCount = 0;
		foreach(ImmutableDataChallenge challengeData in challengeList) {
			if(challengeData.ChallengeType == ChallengeTypes.Regular) {
				GameObject challengeButton = GameObjectUtils.AddChildGUI(challengeGrid.gameObject, challengeButtonPrefab);
				challengeButton.GetComponent<ChallengeButton>().Init(challengeData);
				regularChallengeCount++;
            }
		}

		// Adjust the grid height based on the height of the cell and spacing
		float gridHeight = regularChallengeCount * (challengeGrid.cellSize.y + challengeGrid.spacing.y);
        challengeGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(challengeGrid.cellSize.x, gridHeight);
	}

	public void StartChallenge(string challengeID) {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = challengeID;
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT, showFoodTip: true);
	}
}
