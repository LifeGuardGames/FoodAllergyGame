using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ImmutableDataChallenge  {

	private string id;
	public string ID {
		get { return id; }
	}

	private int tier;
	public int Tier{
		get {return tier;}
	}

	private string challengeMenuSet;		// This is an additive menu set, unlike removeMenuSet
	public string ChallengeMenuSet {
		get { return challengeMenuSet; }
	}

	private string customerSet;
	public string CustomerSet {
		get { return customerSet; }
	}

	private float kitchenTimerMod;
	public float KitchenTimerMod {
		get { return kitchenTimerMod; }
	}

	private float customerTimerMod;
	public float CustomerTimerMod {
		get { return customerTimerMod; }
	}

	private float dayLengthMod;
	public float DayLengthMod {
		get { return dayLengthMod; }
	}

	private string allergy;
	public string Allergy {
		get { return allergy; }
	}

	private string eventDescription;    // Optional
	public string EventDescription {
		get { return eventDescription; }
	}

	private string eventTitle;
	public string EventTitle {
		get { return eventTitle; }
	}

	public float waiterMoveMod;
	public float WaiterMoveMod{
		get{ return waiterMoveMod; }
	}

	public float restMode;
	public float RestMode {
		get { return restMode; }
	}

	public float custSpawnTime;
	public float CustSpawnTime{
		get { return custSpawnTime; }
	}

	private int specialDecoMode;
	public int SpecialDecoMode {
		get { return specialDecoMode; }
	}

	private int startingHearts;
	public int StartingHearts {
		get { return startingHearts; }
	}

	private string nextChall;
	public string NextChall {
		get { return nextChall; }
	}	

	public ImmutableDataChallenge(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
		challengeMenuSet = XMLUtils.GetString(hashElements["ChallengeMenuSet"] as IXMLNode, null, error);
		kitchenTimerMod = XMLUtils.GetFloat(hashElements["KitchenTimer"] as IXMLNode);
		customerSet = XMLUtils.GetString(hashElements["CustomerSet"] as IXMLNode);
		customerTimerMod = XMLUtils.GetFloat(hashElements["CustomerTimer"] as IXMLNode);
		dayLengthMod = XMLUtils.GetFloat(hashElements["DayLength"] as IXMLNode);
		allergy = XMLUtils.GetString(hashElements["Allergy"] as IXMLNode, null, error);

		if(hashElements.Contains("EventDescription")) {
			eventDescription = XMLUtils.GetString(hashElements["EventDescription"] as IXMLNode, "", error);     // Optional
		}
		eventTitle = XMLUtils.GetString(hashElements["Title"] as IXMLNode, null, error);
		waiterMoveMod = XMLUtils.GetFloat(hashElements["WaiterMove"] as IXMLNode);
		restMode = XMLUtils.GetFloat(hashElements["RestMode"] as IXMLNode);
		custSpawnTime = XMLUtils.GetFloat(hashElements["CustomerSpawnTimer"] as IXMLNode);
		specialDecoMode = XMLUtils.GetInt(hashElements["SpecialDecoMode"] as IXMLNode);
		startingHearts = XMLUtils.GetInt(hashElements["CustomerStartingHearts"] as IXMLNode);
		if(hashElements.Contains("NextChallenge")) {
			nextChall = XMLUtils.GetString(hashElements["NextChallenge"] as IXMLNode);
		}
	}
}
