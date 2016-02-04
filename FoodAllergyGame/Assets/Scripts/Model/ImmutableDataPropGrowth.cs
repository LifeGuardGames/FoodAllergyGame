using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataPropGrowth{

	private string id;
	public string ID {
		get { return id; }
	}

	private int tier;
	public int Tier {
		get { return tier; }
	}

	private string propKey;
	public string PropKey {
		get { return propKey; }
	}

	private string prefabName;
	public string PrefabName { 
		get { return prefabName; }
	}

	public ImmutableDataPropGrowth (string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		tier = XMLUtils.GetInt(hashElements["TierUnlock"] as IXMLNode);
		propKey = XMLUtils.GetString(hashElements["PropKey"] as IXMLNode, null, error);
		prefabName = XMLUtils.GetString(hashElements["PrefabName"] as IXMLNode, null, error);
	}

}
