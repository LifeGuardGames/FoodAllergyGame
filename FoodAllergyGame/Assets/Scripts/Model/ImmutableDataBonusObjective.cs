using UnityEngine;
using System.Collections;

public class ImmutableDataBonusObjective {

	private string id;
	public string ID {
		get { return id; }
	}

	private string objType;
	public string ObjType {
		get { return objType; }
	}

	private int num;
	public int Num {
		get { return num; }
	}

	public ImmutableDataBonusObjective(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		objType = XMLUtils.GetString(hashElements["ObjectiveType"] as IXMLNode);
		num = XMLUtils.GetInt(hashElements["Amount"] as IXMLNode);
	}
}
