using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderPropsRandom : XMLLoaderGeneric<DataLoaderPropsRandom> {

	public static ImmutableDataPropRandom GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataPropRandom>(id);
	}

	public static List<ImmutableDataPropRandom> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataPropRandom>();
	}

	public static ImmutableDataPropRandom GetRandomData() {
		List<ImmutableDataPropRandom> dataList = GetDataList();
		int randomIndex = UnityEngine.Random.Range(0, dataList.Count);
		return dataList[randomIndex];
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataPropRandom data = new ImmutableDataPropRandom(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "PropsRandom";
	}
}
