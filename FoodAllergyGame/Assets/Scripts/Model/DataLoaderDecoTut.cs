using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderDecoTut : XMLLoaderGeneric<DataLoaderDecoTut>{

public static ImmutableDataDecoTut GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataDecoTut>(id);
	}
	
	public static List<ImmutableDataDecoTut> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataDecoTut>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataDecoTut data = new ImmutableDataDecoTut(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "DecoTut";
	}
}
