﻿using System.Collections;

public class MutableDataDayTracker{

	public float AvgDifficulty {get; set;}
	public int DaysPlayed {get; set;}

	public MutableDataDayTracker(){
		AvgDifficulty = 0.0f;
		DaysPlayed = 0;
	}
}