using UnityEngine;
using System.Collections;


/// <summary>
/// This class is the decision maker to make the restaurant random
/// </summary>
public class DifficultyAI {
	public DifficultyLevels diff;
	public void Init (float difficultyValue, ImmutableDataEvents eventData) {
		if(difficultyValue < 26) {
			diff = DifficultyLevels.Hard;
		}
		else if (difficultyValue > 33) {
			diff = DifficultyLevels.Easy;
		}
		else {
			diff = DifficultyLevels.Medium;
		}
		RestaurantManagerArcade rest = RestaurantManagerArcade.Instance.GetComponent<RestaurantManagerArcade>();
		rest.AvailableTables(RemoveTables());
		eventData.RestMode = RestModeChooser();
    }

	private int RemoveTables() {
		if(diff == DifficultyLevels.Easy) {
			return 4;
		}
		else if (diff == DifficultyLevels.Medium) {
			int rand = Random.Range(3, 5);
			return rand;
		}
		else {
			int rand = Random.Range(2, 4);
			return rand;
		}
	}

	private int RestModeChooser() {
			int rand = Random.Range(0, 5);
			return rand;
	}
}
