using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataGrowthNode{

	private string id;
	public string ID {
		get { return id; }
	}

	public int tier;
	public int Tier {
		get { return tier; }
	}

	public string propName;
	public string PropName {
		get { return propName; }
	}

	public ImmutableDataGrowthNode (string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
		propName = XMLUtils.GetString(hashElements["PropName"] as IXMLNode, null, error);
	}

}
