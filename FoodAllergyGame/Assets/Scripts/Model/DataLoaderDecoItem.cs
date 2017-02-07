using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderDecoItem: XMLLoaderGeneric<DataLoaderDecoItem> {

	public static ImmutableDataDecoItem GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataDecoItem>(id);
	}
	
	public static List<ImmutableDataDecoItem> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataDecoItem>();
	}

	public static List<string> GetBasicDecoIDUnlockedAtTier(int tier){
		List<string> decoList = new List<string>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()){
			if(decoData.Tier == tier && decoData.DecoTabType != DecoTabTypes.IAP){
				if(decoData.Type == DecoTypes.Kitchen || decoData.Type == DecoTypes.Table || decoData.Type == DecoTypes.Floor){
					decoList.Add(decoData.ID);
				}
			}
		}
		return decoList;
	}

	public static List<string> GetSpecialDecoIDUnlockedAtTier(int tier) {
		List<string> decoList = new List<string>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.Tier == tier && decoData.DecoTabType != DecoTabTypes.IAP) {
				// Get the inverse of basic decorations
				if(decoData.Type != DecoTypes.Kitchen && decoData.Type != DecoTypes.Table && decoData.Type != DecoTypes.Floor) {
					decoList.Add(decoData.ID);
				}
			}
		}
		return decoList;
	}

	// Get all the decos unlocked, filtered by DecoTabTypes, no IAP exclusion necessary
	public static List<string> GetUnlockedDecoListByTabType(int tier, DecoTabTypes decoTabType) {
		List<string> decoList = new List<string>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.Tier <= tier && decoData.DecoTabType == decoTabType) {
				decoList.Add(decoData.ID);
			}
		}
		return decoList;
	}

	// Same as GetUnlockedDecoListByTabType but returns List<ImmutableDataDecoItem>
	public static List<ImmutableDataDecoItem> GetUnlockedDecoDataListByTabType(int tier, DecoTabTypes decoTabType) {
		List<ImmutableDataDecoItem> decoList = new List<ImmutableDataDecoItem>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.Tier <= tier && decoData.DecoTabType == decoTabType) {
				decoList.Add(decoData);
			}
		}
		return decoList;
	}

	// Get all the decos unlocked, excluding IAPs
	public static List<string> GetUnlockedDecoList(int tier) {
		List<string> decoList = new List<string>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.Tier <= tier && decoData.DecoTabType != DecoTabTypes.IAP) {
				decoList.Add(decoData.ID);
			}
		}
		return decoList;
	}

	// IAP items selectable
	public static List<ImmutableDataDecoItem> GetDecoDataByType(DecoTypes type, bool allowIAP = false){
		List<ImmutableDataDecoItem> decoList = new List<ImmutableDataDecoItem>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.Type == type && decoData.Tier <= TierManager.Instance.CurrentTier + 2) {
				if(decoData.DecoTabType == DecoTabTypes.IAP) {
					if(allowIAP) {
						decoList.Add(decoData);
					}
				}
				else {
					decoList.Add(decoData);
				}
			}
		}

		// Sort by Tier
		decoList.Sort((x,y) => x.Tier.CompareTo(y.Tier));
		return decoList;
	}

	public static List<ImmutableDataDecoItem> GetDecoDataByDecoTabType(DecoTabTypes type) {
		List<ImmutableDataDecoItem> decoList = new List<ImmutableDataDecoItem>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.DecoTabType == type && decoData.Tier <= TierManager.Instance.CurrentTier + 2) {
				decoList.Add(decoData);
			}
		}

		// Sort by Tier
		decoList.Sort((x, y) => x.Tier.CompareTo(y.Tier));
		return decoList;
	}

	// Get all the IAPs here, separated out this function for safety
	public static List<ImmutableDataDecoItem> GetDecoDataIAPs() {
		List<ImmutableDataDecoItem> decoList = new List<ImmutableDataDecoItem>();
		foreach(ImmutableDataDecoItem decoData in GetDataList()) {
			if(decoData.DecoTabType == DecoTabTypes.IAP) {
				decoList.Add(decoData);
			}
		}
		return decoList;
	}

	public static string GetDecoIDFromProdID(string prodID) {
	List<ImmutableDataDecoItem> decoList = GetDecoDataIAPs();
		foreach(ImmutableDataDecoItem decoData in decoList) {
			if(prodID == decoData.ProdID) {
				return decoData.ID;
			}
		}
		Debug.LogError("No deco ID from Product ID found: " + prodID);
		return "";
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataDecoItem data = new ImmutableDataDecoItem(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "DecoItem";
	}
}
