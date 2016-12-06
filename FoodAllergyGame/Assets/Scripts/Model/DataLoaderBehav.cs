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

	public static ImmutableDataBehav GetRandomBehavByType(string type) {
		List<ImmutableDataBehav> behavList = GetDataList();
		List<ImmutableDataBehav> finalList = new List<ImmutableDataBehav>();
		foreach(ImmutableDataBehav behav in behavList) {
			if(behav.CustomerType == type) {
				finalList.Add(behav);
			}
		}
		if(finalList.Count == 0) {
			Debug.Log(type + " no Behavs");
			type = "Normal";
			foreach(ImmutableDataBehav behav in behavList) {
				if(behav.CustomerType == type) {
					finalList.Add(behav);
				}
			}
		}
		int rand = Random.Range(0, finalList.Count);
		return finalList[rand];
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
