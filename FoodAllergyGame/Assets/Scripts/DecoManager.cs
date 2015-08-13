using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecoManager : Singleton<DecoManager> {

	public List <ImmutableDataDecoItem> itemList;

	// Use this for initialization
	void Start () {
		itemList = DataLoaderDecoItem.GetDataList();
	}

	//public int getItemCost(string id){
	
	//}
}
