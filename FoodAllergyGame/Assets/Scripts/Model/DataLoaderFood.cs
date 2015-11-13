using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderFood : XMLLoaderGeneric<DataLoaderFood> {

	public static ImmutableDataFood GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataFood>(id);
	}
	
	public static List<ImmutableDataFood> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataFood>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataFood data = new ImmutableDataFood(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "Foods";
	}
}
