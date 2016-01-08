using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DataLoaderChallenge : XMLLoaderGeneric<DataLoaderChallenge> {

	public static ImmutableDataChallenge GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataChallenge>(id);
	}

	public static List<ImmutableDataChallenge> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataChallenge>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataChallenge data = new ImmutableDataChallenge(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "Challenges";
	}
}
