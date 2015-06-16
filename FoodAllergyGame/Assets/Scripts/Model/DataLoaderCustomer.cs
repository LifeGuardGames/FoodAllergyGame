using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DataLoaderCustomer : XMLLoaderGeneric<DataLoaderCustomer> {

	public static ImmutableDataCustomer GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataCustomer>(id);
	}
	
	public static List<ImmutableDataCustomer> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataCustomer>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataCustomer data = new ImmutableDataCustomer(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "Customers";
	}
}
