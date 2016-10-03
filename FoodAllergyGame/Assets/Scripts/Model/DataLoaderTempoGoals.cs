using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderTempoGoals : XMLLoaderGeneric<DataLoaderTempoGoals> {

	public static ImmutableDataTempoGoal GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataTempoGoal>(id);
	}

	public static List<ImmutableDataTempoGoal> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataTempoGoal>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataTempoGoal data = new ImmutableDataTempoGoal(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "TempoGoals";
	}
}
