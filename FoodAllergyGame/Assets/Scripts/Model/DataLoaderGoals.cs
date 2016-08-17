using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderGoals : XMLLoaderGeneric<DataLoaderGoals> {

	public static ImmutableDataGoals GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataGoals>(id);
	}

	public static List<ImmutableDataGoals> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataGoals>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataGoals data = new ImmutableDataGoals(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "Goals";
	}
}
