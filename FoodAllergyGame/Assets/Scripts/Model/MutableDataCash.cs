using System.Collections;

// This class is COMPLETELY encapsulated inside cash manager
// WARNING: DO NOT MAKE DIRECT CALLS WITHOUT CASH MANAGER
public class MutableDataCash {
	public int CurrentCash {get; set;}
	public int TotalCash {get; set;}
	public int LastSeenTotalCash {get; set;}			// Used for animation in the start scene

	public MutableDataCash(){
		// Sync with LastSeenTotalCash in constructor
		CurrentCash = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<int>("CurrentCash") : 0;
		TotalCash = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<int>("TotalCash") : 500;
		LastSeenTotalCash = DataManager.Instance.IsDebug ? Constants.GetDebugConstant<int>("LastTotalCash") : TotalCash;
	}
}
