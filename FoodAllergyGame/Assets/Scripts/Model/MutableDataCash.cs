using System.Collections;

public class MutableDataCash {
	public int CurrentCash {get; set;}
	public int TotalCash {get; set;}
	public int LastSeenTotalCash {get; set;}			// Used for animation in the start scene

	public MutableDataCash(){
		if(DataManager.Instance.IsDebug) {
			CurrentCash = Constants.GetDebugConstant<int>("CurrentCash");
			TotalCash = Constants.GetDebugConstant<int>("TotalCash");
			LastSeenTotalCash = Constants.GetDebugConstant<int>("LastTotalCash");
		}
		else {
			TotalCash = 500;    // Sync with LastSeenTotalCash in constructor
			CurrentCash = 500;
			LastSeenTotalCash = TotalCash;
		}
	}

	public void SaveCash(int dayCashNet, int dayCashRevenue){
		CurrentCash += dayCashNet;
		TotalCash += dayCashRevenue;
	}

	#region Tier Syncing Functions
	// StartManager will check this to see if anyhting
	public bool IsNeedToSyncTotalCash(){
		return LastSeenTotalCash != TotalCash;
	}

	// Called from the NotificationQueueDataTierProgress > StartManager after the tier progress bar HUD is complete
	public void SyncLastSeenTotalCash(){
		LastSeenTotalCash = TotalCash;
	}
	#endregion
}
