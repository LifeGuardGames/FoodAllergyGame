using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderGrowthNode : XMLLoaderGeneric<DataLoaderGrowthNode> {

	public static ImmutableDataGrowthNode GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataGrowthNode>(id);
	}

	public static List<ImmutableDataGrowthNode> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataGrowthNode>();
	}

	public static List<ImmutableDataGrowthNode> GetTypeList(string type) {
		List<ImmutableDataGrowthNode> itemList = GetDataList();
		List<ImmutableDataGrowthNode> currList = new List<ImmutableDataGrowthNode>();
		for (int i = 0; i< itemList.Count; i++) {
			if(itemList[i].PropType == type) {
				currList.Add(itemList[i]);
			}
		}
		return currList;
	}

	public static ImmutableDataGrowthNode GetPropsByTier(int tier, string type) {
		List<ImmutableDataGrowthNode> itemList = GetTypeList(type);
		ImmutableDataGrowthNode curr = GetData ("Growth00");
		for(int i = 0; i<itemList.Count; i++) {
			if(itemList[i].Tier <= tier) {
				if(curr.ID != "Growth00") {
					if(curr.Tier < itemList[i].Tier) {
						curr = itemList[i];
					}
				}
				else {
					curr = itemList[i];
				}
			}
		}
		return curr;
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataGrowthNode data = new ImmutableDataGrowthNode(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "GrowthNode";
	}
}
