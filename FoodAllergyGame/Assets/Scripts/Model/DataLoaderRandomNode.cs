using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderRandomNode : XMLLoaderGeneric<DataLoaderRandomNode> {

	public static ImmutableDataRandomNode GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataRandomNode>(id);
	}

	public static List<ImmutableDataRandomNode> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataRandomNode>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataRandomNode data = new ImmutableDataRandomNode(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "RandomNode";
	}
}
