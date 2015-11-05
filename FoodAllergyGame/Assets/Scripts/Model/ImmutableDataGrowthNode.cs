using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataGrowthNode{

	private string id;
	public string ID {
		get { return id; }
	}

	private int tier;
	public int Tier {
		get { return tier; }
	}

	private string propType;
	public string PropType {
		get { return propType; }
	}

	public ImmutableDataGrowthNode (string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
		propType = XMLUtils.GetString(hashElements["PropType"] as IXMLNode, null, error);
	}

}
