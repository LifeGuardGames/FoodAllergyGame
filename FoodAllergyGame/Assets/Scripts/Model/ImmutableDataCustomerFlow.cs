using UnityEngine;
using System.Collections;

public class ImmutableDataCustomerFlow {

	private string id;
	public string ID {
		get { return id; }
	}

	private string[] flowList;
	public string[] FlowList {
		get { return flowList; }
	}


	public ImmutableDataCustomerFlow(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		flowList = XMLUtils.GetStringList(hashElements["FlowList"] as IXMLNode);
	}
}
