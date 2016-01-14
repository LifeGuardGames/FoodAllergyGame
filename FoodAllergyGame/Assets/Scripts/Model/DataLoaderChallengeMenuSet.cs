using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataLoaderChallengeMenuSet : XMLLoaderGeneric<DataLoaderChallengeMenuSet>{

	public static ImmutableDataChallengeMenuSet GetData(string id){
		instance.InitXMLLoader();
		return instance.GetData<ImmutableDataChallengeMenuSet>(id);
	}
	
	public static List<ImmutableDataChallengeMenuSet> GetDataList(){
		instance.InitXMLLoader();
		return instance.GetDataList<ImmutableDataChallengeMenuSet>();
	}
	
	protected override void XMLNodeHandler(string id, IXMLNode xmlNode, Hashtable hashData, string errorMessage){
		ImmutableDataChallengeMenuSet data = new ImmutableDataChallengeMenuSet(id, xmlNode, errorMessage);
		// Store the data
		if(hashData.ContainsKey(id))
			Debug.LogError(errorMessage + "Duplicate keys!");
		else
			hashData.Add(id, data);
	}
	
	protected override void InitXMLLoader(){
		xmlFileFolderPath = "ChallengeMenuSets";
	}
}