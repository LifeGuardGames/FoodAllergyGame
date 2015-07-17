using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderMenuSet : XMLLoaderGeneric<DataLoaderMenuSet>{

	public static ImmutableDataMenuSet GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataMenuSet>(id);
	}
	
	public static List<ImmutableDataMenuSet> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataMenuSet>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataMenuSet data = new ImmutableDataMenuSet(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "MenuSet";
	}
}