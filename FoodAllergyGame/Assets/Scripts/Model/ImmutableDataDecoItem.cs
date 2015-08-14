using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImmutableDataDecoItem {

	public string id;
	public string ID{
		get{return id;}
	}

	public string type;
	public string Type{
		get {return type;}
	}

	private int cost;
	public int Cost{
		get {return cost;}
	}

	public ImmutableDataDecoItem(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		type = XMLUtils.GetString(hashElements["Type"] as IXMLNode);
		cost = XMLUtils.GetInt(hashElements["Price"] as IXMLNode);
	}
}
