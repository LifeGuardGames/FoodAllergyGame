using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderStartObjects : XMLLoaderGeneric<DataLoaderStartObjects>{

	public static ImmutableDataStartObjects GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataStartObjects>(id);
	}
	
	public static List<ImmutableDataStartObjects> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataStartObjects>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataStartObjects data = new ImmutableDataStartObjects(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "StartObjects";
	}
}
