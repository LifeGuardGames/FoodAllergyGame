﻿using System.Collections;

public class GameData {
	public MutableDataRestaurantEvent RestaurantEvent {get; set;}
	public MutableDataCash Cash {get; set;}
	public MutableDataDayTracker DayTracker {get; set;}
	public MutableDataTutorial Tutorial {get; set;}
	public MutableDataDecoration Decoration {get; set;}

	public GameData(){
		Init();
	}

	private void Init(){
		RestaurantEvent = new MutableDataRestaurantEvent();
		Cash = new MutableDataCash();
		DayTracker = new MutableDataDayTracker();
		Tutorial = new MutableDataTutorial();
		Decoration = new MutableDataDecoration();
	}
}