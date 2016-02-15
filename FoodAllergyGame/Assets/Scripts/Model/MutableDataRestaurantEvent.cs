using System.Collections;
using System.Collections.Generic;

public class MutableDataRestaurantEvent{

	public string CurrentEvent {get; set;}
	public string CurrentChallenge { get; set; }
	public bool ShouldGenerateNewEvent {get; set;}
	public List<string> CustomerList { get; set; }

	public MutableDataRestaurantEvent(){
		CurrentEvent = "EventT3";
		ShouldGenerateNewEvent = true;
		CustomerList = new List<string>();

		// Debug initialize here
		if(DataManager.Instance.IsDebug && Constants.GetDebugConstant<string>("EventID") != default(string)){
			CurrentEvent = Constants.GetDebugConstant<string>("EventID");
			ShouldGenerateNewEvent = false;
		}

		// Debug initialize here
		if(DataManager.Instance.IsDebug && Constants.GetDebugConstant<string>("CustomerSet") != default(string)) {
			string[] customerSet = DataLoaderCustomerSet.GetData(Constants.GetDebugConstant<string>("CustomerSet")).CustomerSet;
            CustomerList = new List<string>(customerSet);
		}
		CurrentChallenge = "Challenge11";
	}
}
