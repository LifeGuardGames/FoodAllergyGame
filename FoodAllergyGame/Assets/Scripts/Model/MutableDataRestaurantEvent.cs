using System.Collections;
using System.Collections.Generic;

public class MutableDataRestaurantEvent{

	public string CurrentEvent {get; set;}
	public bool ShouldGenerateNewEvent {get; set;}

	public MutableDataRestaurantEvent(){
		CurrentEvent = "EventT1";
		ShouldGenerateNewEvent = true;
	}
}
