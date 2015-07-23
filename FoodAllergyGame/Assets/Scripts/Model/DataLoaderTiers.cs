using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderTiers: XMLLoaderGeneric<DataLoaderTiers>{
	
	public static ImmutableDataTiers GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataTiers>(id);
	}
	
	public static List<ImmutableDataTiers> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataTiers>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataTiers data = new ImmutableDataTiers(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "Tiers";
	}
}