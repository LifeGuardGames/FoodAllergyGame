using System.Collections;
using System.Collections.Generic;

public class MutableDataRestaurantEvent{

	public string CurrentEvent {get; set;}
	public bool ShouldGenerateNewEvent {get; set;}
	public List<string> CustomerList;

	public MutableDataRestaurantEvent(){
		CurrentEvent = "EventT1";
		ShouldGenerateNewEvent = true;
		CustomerList = new List<string>();
		// Debug initialize here
		if(DataManager.Instance.IsDebug && Constants.GetDebugConstant<string>("EventID") != default(string)){
			CurrentEvent = Constants.GetDebugConstant<string>("EventID");
			ShouldGenerateNewEvent = false;
		}
	}
}
