using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataEvents{

	private string id;
	public string ID{
		get{ return id; }
	}

	private string menuSet;
	public string MenuSet{
		get{return menuSet;}
	}

	private string kitchenTimerMod;
	public string KitchenTimerMod{
		get{return kitchenTimerMod;}
	}

	private string customerTimerMod;
	public string CustomerTimerMod{
		get{return customerTimerMod;}
	}

	private string dayLengthMod;
	public string DayLengthMod{
		get{return dayLengthMod;}
	}

	private string allergy;
	public string Allergy{
		get {return allergy;}
	}

	public ImmutableDataEvents(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		menuSet = XMLUtils.GetString(hashElements["MenuSet"] as IXMLNode, null, error);
		kitchenTimerMod = XMLUtils.GetString(hashElements["KitchenTimer"] as IXMLNode, null, error);
		customerTimerMod = XMLUtils.GetString(hashElements["CustomerTimer"] as IXMLNode, null, error);
		dayLengthMod = XMLUtils.GetString(hashElements["DayLength"] as IXMLNode, null, error);
		allergy = XMLUtils.GetString(hashElements["Allergy"] as IXMLNode, null , error);
	}
}
