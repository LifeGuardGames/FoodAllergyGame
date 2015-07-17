using System.Collections;
using System.Collections.Generic;

public class MutableDataRestaurantEvent{

	public string CurrentEvent {get; set;}
	public List<string> menuPlanningStock {get; set;}	// The final food stock that will go to populate menu planning scene

	public MutableDataRestaurantEvent(){
		CurrentEvent = "Event00";
		menuPlanningStock = new List<string>();
	}
}
