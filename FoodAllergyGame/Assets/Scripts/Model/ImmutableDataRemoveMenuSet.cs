using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataRemoveMenuSet{

	private string id;
	public string ID{
		get { return id;}
	}

	private string[] removeMenuSet;
	public string[] RemoveMenuSet {
		get{ return removeMenuSet; }
	}

	public ImmutableDataRemoveMenuSet(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		removeMenuSet = XMLUtils.GetStringList(hashElements["FoodList"]as IXMLNode);
	}
}
