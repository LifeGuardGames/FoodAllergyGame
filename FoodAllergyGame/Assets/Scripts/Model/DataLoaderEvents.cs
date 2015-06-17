using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderEvents : XMLLoaderGeneric<DataLoaderEvents>{

	public static ImmutableDataEvents GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataEvents>(id);
	}

	public static List<ImmutableDataEvents> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataEvents>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataEvents data = new ImmutableDataEvents(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "Events";
	}
}
