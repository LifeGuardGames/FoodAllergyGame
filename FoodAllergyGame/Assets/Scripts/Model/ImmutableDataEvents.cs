using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataEvents{

	private string id;
	public string ID{
		get{ return id; }
	}

	private string menuMod;
	public string MenuMod{
		get{return menuMod;}
	}

	private string kitchenMod;
	public string KitchenMod{
		get{return kitchenMod;}
	}

	private string customerMod;
	public string CustomerMod{
		get{return customerMod;}
	}

	private string dayMod;
	public string DayMod{
		get{return dayMod;}
	}

	private string allergy;
	public string Allergy{
		get {return allergy;}
	}
	
	public ImmutableDataEvents(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		menuMod = XMLUtils.GetString(hashElements["Menu"] as IXMLNode, null, error);
		kitchenMod = XMLUtils.GetString(hashElements["Kitchen"] as IXMLNode, null, error);
		customerMod = XMLUtils.GetString(hashElements["Customer"] as IXMLNode, null, error);
		dayMod = XMLUtils.GetString(hashElements["Day"] as IXMLNode, null, error);
		allergy = XMLUtils.GetString(hashElements["Allergy"] as IXMLNode, null , error);
	}
}
