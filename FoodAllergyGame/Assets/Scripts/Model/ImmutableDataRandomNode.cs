using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataRandomNode {

	private string id;
	public string ID {
		get { return id; }
	}

	public int number;
	public int Number {
		get { return number; }
	}

	public string propName;
	public string PropName {
		get { return propName; }
	}

	public ImmutableDataRandomNode(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		number = XMLUtils.GetInt(hashElements["number"] as IXMLNode);
		propName = XMLUtils.GetString(hashElements["PropName"] as IXMLNode, null, error);
	}

}
