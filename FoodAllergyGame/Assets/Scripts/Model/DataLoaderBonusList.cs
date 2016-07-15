using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderBonusList : XMLLoaderGeneric<DataLoaderBonusList> {

	public static ImmutableDataBonusList GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataBonusList>(id);
	}

	public static List<ImmutableDataBonusList> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataBonusList>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataBonusList data = new ImmutableDataBonusList(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "BonusList";
	}
}
