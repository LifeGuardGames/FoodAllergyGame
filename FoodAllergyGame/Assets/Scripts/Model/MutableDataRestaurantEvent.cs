using System.Collections;
using System.Collections.Generic;

public class MutableDataRestaurantEvent{

	public string CurrentEvent {get; set;}
	public List<string> MenuPlanningStock {get; set;}	// The final food stock that will go to populate menu planning scene
	public bool ShouldGenerateNewEvent {get; set;}

	public MutableDataRestaurantEvent(){
		CurrentEvent = "Event00";
		MenuPlanningStock = new List<string>();
		ShouldGenerateNewEvent = true;
	}
}
