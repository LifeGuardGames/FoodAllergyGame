using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderPropGrowth : XMLLoaderGeneric<DataLoaderPropGrowth> {

	public static ImmutableDataPropGrowth GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataPropGrowth>(id);
	}

	public static List<ImmutableDataPropGrowth> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataPropGrowth>();
	}

	// Given a tier and their propKey, get the data to load
	public static ImmutableDataPropGrowth GetPropsByTier(int currentTier, string propKey) {
		List<ImmutableDataPropGrowth> itemList = GetTypeList(propKey);
		ImmutableDataPropGrowth curr = null;
		for(int i = 0; i < itemList.Count; i++) {
			if(itemList[i].Tier <= currentTier) {
				if(curr == null || curr.Tier < itemList[i].Tier) {
					curr = itemList[i];
				}
			}
		}
		return curr;
	}

	// Filter all grows props by their propKey
	private static List<ImmutableDataPropGrowth> GetTypeList(string propKey) {
		List<ImmutableDataPropGrowth> itemList = GetDataList();
		List<ImmutableDataPropGrowth> currList = new List<ImmutableDataPropGrowth>();
		for(int i = 0; i < itemList.Count; i++) {
			if(itemList[i].PropKey == propKey) {
				currList.Add(itemList[i]);
			}
		}
		return currList;
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataPropGrowth data = new ImmutableDataPropGrowth(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "PropsGrowth";
	}
}
