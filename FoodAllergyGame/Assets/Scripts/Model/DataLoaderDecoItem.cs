using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderDecoItem: XMLLoaderGeneric<DataLoaderDecoItem> {

	public static ImmutableDataDecoItem GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataDecoItem>(id);
	}
	
	public static List<ImmutableDataDecoItem> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataDecoItem>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataDecoItem data = new ImmutableDataDecoItem(id, xmlNode, errorMessage);
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
