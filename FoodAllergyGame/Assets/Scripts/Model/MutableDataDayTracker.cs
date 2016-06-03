﻿using System.Collections.Generic;

public class MutableDataDayTracker {
	public float AvgDifficulty { get; set; }
	public int DaysPlayed { get; set; }
	public int ChallengesPlayed {get;set;}
	public bool IsMoreCrates { get; set; }
	public bool HasCollectedAge { get; set; }
	public List<string> notifQueue { get; set; }

	public MutableDataDayTracker(){
		AvgDifficulty = 15.0f;
		DaysPlayed = 0;
		ChallengesPlayed = 0;
		
		IsMoreCrates = true;    //pro toggle
		HasCollectedAge = false;
		notifQueue = new List<string>();
	}
}
