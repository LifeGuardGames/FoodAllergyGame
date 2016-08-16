using UnityEngine;
using System.Collections;

public class TempoLogicController : MonoBehaviour {


	public ImmutableDataGoals GetCurrentGoal() {
		return DataLoaderGoals.GetData(DataManager.Instance.GameData.DayTracker.currentGoal);
	}

	public void GetNewGoal() {
		int tier = TierManager.Instance.CurrentTier;
		// TODO talk about mid goals or randomized goals
		ImmutableDataGoals gol = DataLoaderGoals.GetData("Goal_" + tier);
        DataManager.Instance.GameData.DayTracker.currentGoal = gol.ID;
		DataManager.Instance.GameData.DayTracker.goalTimeLimit = gol.TimeLimit;
	}

	public bool CheckProgress() {
		int tCash = CashManager.Instance.TotalCash;
		if(tCash < DataLoaderGoals.GetData(DataManager.Instance.GameData.DayTracker.currentGoal).GoalPoint) {
			if(DataManager.Instance.GameData.DayTracker.goalTimeLimit == 0) {
				//goalfailed
				DataManager.Instance.GameData.DayTracker.currentGoal = "";
				return false;
			}
			return false;
		}
		else {
			return true;
		}
	}

	public int GetDifferenceInGoal() {
		if(!CheckProgress()) {
			int tCash = CashManager.Instance.TotalCash;
			return DataLoaderGoals.GetData(DataManager.Instance.GameData.DayTracker.currentGoal).GoalPoint - tCash;
		}
		else {
			return 0;
		}
	}

}
