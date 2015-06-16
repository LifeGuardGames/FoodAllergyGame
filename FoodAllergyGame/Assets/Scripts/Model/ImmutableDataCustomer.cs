using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataCustomer{

	private string id;
	public string ID{
		get{ return id; }
	}

	private string script;
	public string Script{
		get{return script;}
	}

	public ImmutableDataCustomer(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		script = XMLUtils.GetString(hashElements["Script"] as IXMLNode, null, error);
	}
}
