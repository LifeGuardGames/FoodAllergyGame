using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataBehav  {

	private string id;
	public string ID {
		get { return id; }
	}

	private string[] behav;
	public string[] Behav {
		get { return behav; }
	}

	private string name;
	public string Name {
		get { return name; }
	}

	private string customerType;
	public string CustomerType {
		get { return customerType; }
	}

	public ImmutableDataBehav(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		name = XMLUtils.GetString(hashElements["Name"] as IXMLNode);
		customerType = XMLUtils.GetString(hashElements["Type"] as IXMLNode);
		behav = XMLUtils.GetStringList(hashElements["BehavList"]as IXMLNode);
	}
}
