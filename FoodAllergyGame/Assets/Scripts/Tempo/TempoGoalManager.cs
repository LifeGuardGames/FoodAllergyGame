public class TempoGoalManager : Singleton<TempoGoalManager> {
	
	public ImmutableDataTempoGoal GetCurrentGoal() {
		return DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal);
	}

	// Call on new tier
	public void GetNewGoal() {
		int tier = TierManager.Instance.CurrentTier;
		// TODO talk about mid goals or randomized goals
		ImmutableDataTempoGoal tempoGoalData = DataLoaderTempoGoals.GetData("TempoGoal_" + tier);
        DataManager.Instance.GameData.DayTracker.CurrentTempoGoal = tempoGoalData.ID;
		DataManager.Instance.GameData.DayTracker.GoalTimeLimit = tempoGoalData.TimeLimit;
	}

	// Completed or not
	public bool IsGoalCompleted() {
		int tCash = CashManager.Instance.TotalCash;
		if(GetPercentageOfTier(tCash) < DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPoint) {
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
	}

	// How much more totalCash needed to get goal
	public int GetDifferenceInGoal() {
		if(!IsGoalCompleted()) {
			int tCash = CashManager.Instance.TotalCash;
			return DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPoint - tCash;
		}
		else {
			return 0;
		}
	}

	// Percentage at current tier bracket where it is located
	public float GetPercentageInCurrentTier() {
		if(!IsGoalCompleted()) {
			int tCash = CashManager.Instance.TotalCash;
			return (float)tCash / DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPoint;
		}
		else {
			return 0;
		}
	}

	public float GetPercentageOfTier(int tcash) {
		return ((float)tcash /DataLoaderTiers.GetDataFromTier(TierManager.Instance.CurrentTier).CashCutoffFloor) *100;
	}
}
