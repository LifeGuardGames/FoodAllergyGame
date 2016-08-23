using UnityEngine;

public class TempoGoalManager : Singleton<TempoGoalManager> {
	
	public ImmutableDataTempoGoal GetCurrentGoal() {
		return DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal);
	}

	// Call on new tier if applicable
	public void GetNewGoal() {
		int tier = TierManager.Instance.CurrentTier;
		if(tier >= 1) {
			Debug.Log("New Goal Set");
			// TODO talk about mid goals or randomized goals
			ImmutableDataTempoGoal tempoGoalData = DataLoaderTempoGoals.GetData("TempoGoal_" + tier);
			DataManager.Instance.GameData.DayTracker.CurrentTempoGoal = tempoGoalData.ID;
			DataManager.Instance.GameData.DayTracker.GoalTimeLimit = tempoGoalData.TimeLimit;
		}
	}

	// Completed or not, calculated on the fly based on goalpost instead of saved status
	public bool ShouldDisplayGoal() {
		return false;////////
		int oldTotalCash = TierManager.Instance.OldTotalCash;
		int tCash = CashManager.Instance.TotalCash;
		string currentGoal = DataManager.Instance.GameData.DayTracker.CurrentTempoGoal;
		if(!string.IsNullOrEmpty(currentGoal)) {        // Goal not set yet
			//ImmutableDataTempoGoal goalData = DataLoaderTempoGoals.GetData(currentGoal);
			//ImmutableDataTiers currentTier = DataLoaderTiers.GetTierFromCash(oldTotalCash);
   //         if(currentTier == goalData.Tier) {
			//	if(DataLoaderTiers.GetCashInTier())
			//}
		//	else {
		//		Debug.LogError("Wrong tier for tempoGoal");
	//		}
		//	return false;
		}
		else {
			return false;
		}
	}

	public void RewardGoalComplete() {

	}

	// How much more totalCash needed to get goal
	public int GetDifferenceInGoal() {
		//if(!ShouldDisplayGoal()) {
		//	int tCash = CashManager.Instance.TotalCash;
		//	return DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPointTierPercentage - tCash;
		//}
		//else {
			return 0;
		//}
	}

	// Percentage at current tier bracket where it is located
	public float GetPercentageInCurrentTier() {
		string currentGoal = DataManager.Instance.GameData.DayTracker.CurrentTempoGoal;
        if(!ShouldDisplayGoal() && !string.IsNullOrEmpty(currentGoal)) {
			int tCash = CashManager.Instance.TotalCash;
			Debug.Log("DFD" + (float)tCash / DataLoaderTempoGoals.GetData(currentGoal).GoalPointTierPercentage);
			return (float)tCash / DataLoaderTempoGoals.GetData(DataManager.Instance.GameData.DayTracker.CurrentTempoGoal).GoalPointTierPercentage;
		}
		else {
			Debug.Log("COMPLETED or null");
			return 0;
		}
	}

	public float GetPercentageOfTier(int tcash) {
		return ((float)tcash /DataLoaderTiers.GetDataFromTier(TierManager.Instance.CurrentTier).CashCutoffFloor) *100;
	}
}
