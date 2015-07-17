using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderCustomerSet : XMLLoaderGeneric<DataLoaderCustomerSet>{
	
	public static ImmutableDataCustomerSet GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataCustomerSet>(id);
	}
	
	public static List<ImmutableDataCustomerSet> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataCustomerSet>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataCustomerSet data = new ImmutableDataCustomerSet(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "CustomerSets";
	}
}
