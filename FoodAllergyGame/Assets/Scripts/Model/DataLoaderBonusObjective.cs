using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderBonusObjective : XMLLoaderGeneric<DataLoaderBonusObjective> {

	public static ImmutableDataBonusObjective GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataBonusObjective>(id);
	}

	public static List<ImmutableDataBonusObjective> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataBonusObjective>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataBonusObjective data = new ImmutableDataBonusObjective(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "BonusObjectives";
	}
}
