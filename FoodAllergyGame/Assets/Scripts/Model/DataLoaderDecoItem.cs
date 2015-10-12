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

	public static List<ImmutableDataDecoItem> GetDecoDataByType(DecoTypes type){
		List<ImmutableDataDecoItem> itemList = GetDataList();
		List<ImmutableDataDecoItem> decoList = new List<ImmutableDataDecoItem>();
		for(int i = 0; i < itemList.Count; i++){
			if(itemList[i].Type == type && itemList[i].Tier == TierManager.Instance.Tier){
				decoList.Add(itemList[i]);
			}
		}

		// Sort by cost
		decoList.Sort((x,y) => x.ID.CompareTo(y.ID));
		return decoList;
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
		xmlFileFolderPath = "DecoItem";
	}
}
