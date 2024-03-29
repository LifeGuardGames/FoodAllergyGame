﻿using UnityEngine;

/// <summary>
/// Cash manager.
/// All changes to mutable data coin MUST but called through here
/// </summary>
public class CashManager : Singleton<CashManager> {

	MutableDataCash cashData;
	public int CurrentCash{ get{ return cashData.CurrentCash; } }
	public int TotalCash{ get{ return cashData.TotalCash; } }
	public int LastSeenTotalCash{ get{ return cashData.LastSeenTotalCash; } }

	void Awake() {
		cashData = DataManager.Instance.GameData.Cash;
	}

	void Start() {
		if(LastSeenTotalCash > TotalCash) {
			Debug.LogError("LastSeenTotalCash is greater than TotalCash");
		}
	}

	public void OverrideTotalCash(int newCash) {
		cashData.TotalCash = newCash;
	}

	public void OverrideCurrentCash(int cash, Vector3 loc) {
		cashData.CurrentCash += cash;
		if(HUDAnimator.Instance != null) {
			HUDAnimator.Instance.CashAnimationStart(cash, loc);
		}
	}


	#region DecoScene calls
	// Wrap this in another layer for extra control
	public bool DecoBuyCash(int cost){
		if(cost < 0){
			Debug.LogError("Cant process negative cost");
			return false;
		}

		if(cost <= CurrentCash){
			cashData.CurrentCash -= cost;
			if(HUDAnimator.Instance != null){
				HUDAnimator.Instance.CashAnimationStart(-cost, GameObject.Find("ButtonBuy").transform.position);
			}
			return true;
		}
		return false;
	}
	#endregion

	#region RestaurantScene calls
	public void RestaurantEndCashUpdate(int dayCashNet, int dayCashRevenue){
		if(dayCashRevenue < 0){
			Debug.LogError("Negative end day revenue detected");
		}
		else {
			cashData.TotalCash += dayCashRevenue;
		}

		cashData.CurrentCash += dayCashNet;

		if(cashData.CurrentCash < 0){
			Debug.LogWarning("Current cash below 0, resetting to 0");
			cashData.CurrentCash = 0;
		}
	}

	public void TutorialOverrideTotalCash(int totalCashOverride){
		cashData.TotalCash = totalCashOverride;
	}
	#endregion

	#region StartScene calls
	// StartManager will check this to see if anyhting
	public bool IsNeedToSyncTotalCash(){
		return cashData.LastSeenTotalCash != cashData.TotalCash;
	}

	// Called from the NotificationQueueDataTierProgress > StartManager after the tier progress bar HUD is complete
	public void SyncLastSeenTotalCash(){
		cashData.LastSeenTotalCash = cashData.TotalCash;
	}
	#endregion
}
