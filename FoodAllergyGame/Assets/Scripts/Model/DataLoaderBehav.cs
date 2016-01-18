using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderBehav : XMLLoaderGeneric<DataLoaderBehav> {

	public static ImmutableDataBehav GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataBehav>(id);
	}

	public static List<ImmutableDataBehav> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataBehav>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataBehav data = new ImmutableDataBehav(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "Behav";
	}
}
