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

	private string script;
	public string Script{
		get{return script;}
	}

	public ImmutableDataCustomer(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		customerNameKey = XMLUtils.GetString(hashElements["CustomerNameKey"] as IXMLNode, null, error);

		script = XMLUtils.GetString(hashElements["Script"] as IXMLNode, null, error);

	}
}
