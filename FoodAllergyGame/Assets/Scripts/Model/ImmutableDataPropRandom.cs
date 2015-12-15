using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataPropRandom {

	private string id;
	public string ID {
		get { return id; }
	}

	private string prefabName;
	public string PrefabName {
		get { return prefabName; }
	}

	public ImmutableDataPropRandom(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		prefabName = XMLUtils.GetString(hashElements["PrefabName"] as IXMLNode, null, error);
	}

}
