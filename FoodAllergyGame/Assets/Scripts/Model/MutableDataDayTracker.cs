using System.Collections.Generic;

public class MutableDataDayTracker {
	public float AvgDifficulty { get; set; }
	public int DaysPlayed { get; set; }
	public int ChallengesPlayed {get;set;}
	public bool IsMoreCrates { get; set; }
	public bool HasCollectedAge { get; set; }
	public List<string> NotifQueue { get; set; }
	public string CurrentTempoGoal { get; set; }
	public int GoalTimeLimit { get; set; }
	public bool HasCompletedGoalThisTier { get; set; }
	public int IAPCurrency { get; set; }
	public bool IsAmazonUnderground { get; set; }

	public MutableDataDayTracker(){
		AvgDifficulty = 15.0f;
		DaysPlayed = 0;
		ChallengesPlayed = 0;
		//pro toggle
		IsMoreCrates = true;
		HasCollectedAge = false;
		NotifQueue = new List<string>();
		CurrentTempoGoal = "";
		HasCompletedGoalThisTier = false;
		IAPCurrency = 0;
		IsAmazonUnderground = false;
	}
}
