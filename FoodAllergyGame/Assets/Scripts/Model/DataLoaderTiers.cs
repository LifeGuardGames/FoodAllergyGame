using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderTiers: XMLLoaderGeneric<DataLoaderTiers>{
	
	public static ImmutableDataTiers GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataTiers>(id);
	}

	// Sorted by CashCutoffFloor for calculation purposes
	public static List<ImmutableDataTiers> GetDataList(){
		instance.InitXMLLoader();

		// Sort not working....
		//instance.GetDataList<ImmutableDataTiers>().Sort((x,y) => x.CashCutoffFloor.CompareTo(y.CashCutoffFloor));
		return instance.GetDataList<ImmutableDataTiers>();
	}

	public static int GetTotalTierCount() {
		return GetDataList().Count - 1;	// Disregard tutorial tier
	}

	public static ImmutableDataTiers GetDataFromTier(int tier) {
		return GetData("Tier" + StringUtils.FormatIntToDoubleDigitString(tier));
	}

	public static ImmutableDataTiers GetNextTier(ImmutableDataTiers tierData) {
		if(!tierData.IsLastTier) {
			return GetDataFromTier(tierData.TierNumber + 1);
		}
		else {
			return null;
		}
    }

	public static int GetTierFromCash(int totalCash){
		List<ImmutableDataTiers> tierList = GetDataList();
		int tierSoFar = 0;
		for(int i = 0; i < tierList.Count; i++){
			if(tierList[i].CashCutoffFloor <= totalCash && tierSoFar < tierList[i].TierNumber) {
				tierSoFar = tierList[i].TierNumber;
			}
		}
		return tierSoFar;
	}

	public static float GetPercentProgressInTier(int totalCash){
		int tier = GetTierFromCash(totalCash);
		ImmutableDataTiers tierData = GetDataFromTier(tier);
		if(!tierData.IsLastTier) {
			ImmutableDataTiers nextTierData = GetDataFromTier(tier + 1);
			int tierCashDifference = nextTierData.CashCutoffFloor - tierData.CashCutoffFloor;
			int cashInTier = totalCash - tierData.CashCutoffFloor;
			float percentage = (float)cashInTier / (float)tierCashDifference;
			return percentage;
		}
		else {  // Last tier, just return 1
			Debug.LogWarning("Last tier calculated, check logic!");
			return 1f;
		}
	}

	/// <summary>
	/// Check if you earned slot spaces at the current tier, compared to the last tier
	/// </summary>
	public static int GetSlotsUnlockedAtTier(int tier){
		if(tier == 0){
			Debug.LogWarning("Getting delta tier at first tier");
			return 0;
		}

		ImmutableDataTiers currentTier = GetDataFromTier(tier);
		ImmutableDataTiers previousTier = GetDataFromTier(tier - 1);

		return currentTier.MenuSlots - previousTier.MenuSlots;
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataTiers data = new ImmutableDataTiers(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "Tiers";
	}
}