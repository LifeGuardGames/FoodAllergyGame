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
