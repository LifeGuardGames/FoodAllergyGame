using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderRemoveMenuSet : XMLLoaderGeneric<DataLoaderRemoveMenuSet>{

	public static ImmutableDataRemoveMenuSet GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataRemoveMenuSet>(id);
	}
	
	public static List<ImmutableDataRemoveMenuSet> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataRemoveMenuSet>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataRemoveMenuSet data = new ImmutableDataRemoveMenuSet(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "RemoveMenuSets";
	}
}