using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataMenuSet{

	private string id;
	public string ID{
		get { return id;}
	}

	private string[] menuSet;
	public string[] MenuSet {
		get{ return menuSet; }
	}

	public ImmutableDataMenuSet(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		menuSet = XMLUtils.GetStringList(hashElements["FoodList"]as IXMLNode);
	}
}
