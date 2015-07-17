using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataCustomer{

	private string id;
	public string ID{
		get{ return id; }
	}

	private string customerNameKey;
	public string CustomerNameKey{
		get{ return customerNameKey; }
	}

	private string customerDescription;
	public string CustomerDescription{
		get{ return customerDescription; }
	}

	private string spriteName;
	public string SpriteName{
		get{ return spriteName; }
	}

	private string script;
	public string Script{
		get{return script;}
	}

	private int tier;
	public int Tier{
		get{return tier;}
	}

	public ImmutableDataCustomer(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		customerNameKey = XMLUtils.GetString(hashElements["CustomerNameKey"] as IXMLNode, null, error);
		customerDescription = XMLUtils.GetString(hashElements["CustomerDesc"] as IXMLNode, null, error);
		spriteName = XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode, null, error);
		script = XMLUtils.GetString(hashElements["Script"] as IXMLNode, null, error);
		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
	}
}
