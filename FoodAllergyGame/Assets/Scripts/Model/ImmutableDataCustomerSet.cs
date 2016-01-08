using UnityEngine;
using System.Collections;

public class ImmutableDataCustomerSet {

	private string id;
	public string ID{
		get { return id;}
	}
	
	private string[] customerSet;
	public string[] CustomerSet {
		get{ return customerSet; }
	}

	public ImmutableDataCustomerSet(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		customerSet = XMLUtils.GetStringList(hashElements["CustomerList"] as IXMLNode);
	}
}
