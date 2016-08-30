public class TempoGoalManager : Singleton<TempoGoalManager> {
	
	public ImmutableDataTempoGoal GetCurrentGoal() {
		return DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal);
	}

	// Call on new tier
	public void GetNewGoal() {
		DataManager.Instance.GameData.DayTracker.HasCompletedGoalThisTier = false;
		int tier = TierManager.Instance.CurrentTier;
		// TODO talk about mid goals or randomized goals
		ImmutableDataTempoGoal tempoGoalData = DataLoaderTempoGoals.GetData("TempoGoal_" + tier);
        DataManager.Instance.GameData.DayTracker.CurrentTempoGoal = tempoGoalData.ID;
		DataManager.Instance.GameData.DayTracker.GoalTimeLimit = tempoGoalData.TimeLimit;
	}

	// Completed or not
	/*
	public bool IsGoalCompleted() {
		int tCash = CashManager.Instance.TotalCash;
		if(GetPercentageOfTier(tCash) < DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPointTierPercentage) {
			if(DataManager.Instance.GameData.DayTracker.GoalTimeLimit == 0) {
				//goalfailed
				DataManager.Instance.GameData.DayTracker.CurrentTempoGoal = "";
				return false;
			}
			return false;
		}
		else {
			return true;
		}
	}*/

	// Percentage at current tier bracket where it is located
	/*
	public float GetPercentageInTotal() {
		if(!IsGoalCompleted()) {
			int tCash = CashManager.Instance.TotalCash;
			return (float)tCash / DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPointTierPercentage;
		}
		else {
			return 0;
		}
	}*/

	public float GetPercentageComet() {
		return ((float)DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPointTierPercentage);
	}

	public float GetPercentageOfTier(int tcash) {
		return ((float)tcash /DataLoaderTiers.GetDataFromTier(TierManager.Instance.CurrentTier).CashCutoffFloor) *100;
	}
}
