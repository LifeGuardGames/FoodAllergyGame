using System.Collections;

public class MutableDataDayTracker {

	public float AvgDifficulty { get; set; }
	public int DaysPlayed { get; set; }
	public int ChallengesPlayed {get;set;}

	public MutableDataDayTracker(){
		AvgDifficulty = 15.0f;
		DaysPlayed = 0;
		ChallengesPlayed = 0;
	}
}
