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
		instance.GetDataList<ImmutableDataTiers>().Sort((x,y) => x.CashCutoffFloor.CompareTo(y.CashCutoffFloor));
		return instance.GetDataList<ImmutableDataTiers>();
	}

	public static int GetTierFromCash(int totalCash){
		List<ImmutableDataTiers> tierList = GetDataList();

		int tierSoFar = 0;
		for(int i = 0; i < tierList.Count; i++){
			if(tierList[i].CashCutoffFloor <= totalCash){
				tierSoFar = tierList[i].TierNumber;
			}
		}
		return tierSoFar;
	}

	public static float GetPercentProgressInTier(int totalCash){
		List<ImmutableDataTiers> tierList = GetDataList();
		
		int tierSoFar = 0;
		float percentage = 0f;
		for(int i = 0; i < tierList.Count; i++){
			if(tierList[i].CashCutoffFloor <= totalCash && tierSoFar <= tierList[i].TierNumber) {
				tierSoFar = tierList[i].TierNumber;
				// Determine if this is the last tier
				if(!tierList[i].IsLastTier) {
					int tierCashDifference = tierList[i + 1].CashCutoffFloor - tierList[i].CashCutoffFloor;
					int cashInTier = totalCash - tierList[i].CashCutoffFloor;
					percentage = (float)cashInTier / (float)tierCashDifference;
				}
				else{		// Last element on the list
					percentage = -1f;
				}
			}
		}
		return percentage;
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