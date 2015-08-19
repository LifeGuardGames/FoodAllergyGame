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

	private string customerSet;
	public string CustomerSet{
		get{return customerSet;}
	}

	private float kitchenTimerMod;
	public float KitchenTimerMod{
		get{return kitchenTimerMod;}
	}

	private float customerTimerMod;
	public float CustomerTimerMod{
		get{return customerTimerMod;}
	}

	private float dayLengthMod;
	public float DayLengthMod{
		get{return dayLengthMod;}
	}

	private string allergy;
	public string Allergy{
		get {return allergy;}
	}

	private string eventDescription;	// Optional
	public string EventDescription{
		get{return eventDescription;}
	}

	public ImmutableDataEvents(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		menuSet = XMLUtils.GetString(hashElements["MenuSet"] as IXMLNode, null, error);
		kitchenTimerMod = XMLUtils.GetFloat(hashElements["KitchenTimer"] as IXMLNode);
		customerSet = XMLUtils.GetString(hashElements["CustomerSet"] as IXMLNode);
		customerTimerMod = XMLUtils.GetFloat(hashElements["CustomerTimer"] as IXMLNode);
		dayLengthMod = XMLUtils.GetFloat(hashElements["DayLength"] as IXMLNode);
		allergy = XMLUtils.GetString(hashElements["Allergy"] as IXMLNode, null , error);

		if(hashElements.Contains("EventDescription")){
			eventDescription = XMLUtils.GetString(hashElements["EventDescription"] as IXMLNode, "", error);		// Optional
		}
	}
}
