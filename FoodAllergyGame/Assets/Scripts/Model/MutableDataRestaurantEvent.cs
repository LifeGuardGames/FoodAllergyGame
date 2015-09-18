using System.Collections;
using System.Collections.Generic;

public class MutableDataRestaurantEvent{

	public string CurrentEvent {get; set;}
	public bool ShouldGenerateNewEvent {get; set;}

	public MutableDataRestaurantEvent(){
		CurrentEvent = "EventT1";
		ShouldGenerateNewEvent = true;

		// Debug initialize here
		if(DataManager.Instance.IsDebug && Constants.GetDebugConstant<string>("EventID") != default(string)){
			CurrentEvent = Constants.GetDebugConstant<string>("EventID");
			ShouldGenerateNewEvent = false;
		}
	}
}
