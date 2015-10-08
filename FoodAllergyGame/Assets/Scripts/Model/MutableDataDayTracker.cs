using System.Collections;

public class MutableDataDayTracker{

	public float AvgDifficulty {get; set;}
	public int DaysPlayed {get; set;}

	public MutableDataDayTracker(){
		AvgDifficulty = 10.0f;
		DaysPlayed = 0;
	}
}
