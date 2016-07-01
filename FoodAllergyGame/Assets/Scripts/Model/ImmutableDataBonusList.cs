using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImmutableDataBonusList {

	private string id;
	public string ID {
		get { return id; }
	}

	private string[] objs;
	public string[] Objs {
		get { return objs; }
	}
	public ImmutableDataBonusList(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		objs = XMLUtils.GetStringList(hashElements["List"] as IXMLNode);
	}
}
