using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderCustomerFlow : XMLLoaderGeneric<DataLoaderCustomerFlow> {

	public static ImmutableDataCustomerFlow GetData(string id) {
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataCustomerFlow>(id);
	}

	public static List<ImmutableDataCustomerFlow> GetDataList() {
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataCustomerFlow>();
	}

	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage) {
		ImmutableDataCustomerFlow data = new ImmutableDataCustomerFlow(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}

	protected override void InitXMLLoader() {
		xmlFileFolderPath = "CustomerFlow";
	}
}