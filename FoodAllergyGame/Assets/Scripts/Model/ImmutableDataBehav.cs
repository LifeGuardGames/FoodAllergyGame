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

	public ImmutableDataBehav(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		behav = XMLUtils.GetStringList(hashElements["BehavList"]as IXMLNode);
	}
}
