using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ImmutableDataChallenge  {

	private string id;
	public string ID {
		get { return id; }
	}

	private ChallengeTypes challengeType;
	public ChallengeTypes ChallengeType {
		get { return challengeType; }
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

	private string title;
	public string Title {
		get { return title; }
	}

	private string challengeDescription;    // Optional
	public string ChallengeDescription {
		get { return challengeDescription; }
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

	private int bronzeBreakPoint;
	public int BronzeBreakPoint {
	get { return bronzeBreakPoint; }
	}

	private int silverBreakPoint;
	public int SilverBreakPoint {
		get { return silverBreakPoint; }
	}

	private int goldBreakPoint;
	public int GoldBreakPoint {
		get { return goldBreakPoint; }
	}

	private int numOfTables;
	public int NumOfTables {
		get { return numOfTables; }
	}

	private int gossiperMode;
	public int GossiperMode {
		get { return gossiperMode; }
	}

	private string playArea;
	public string PlayArea {
		get { return playArea; }
	}

	private string vipTable;
	public string VipTable {
		get { return vipTable; }
	}

	private string flyThru;
	public string FlyThru {
		get { return flyThru; }
	}

	private bool isBossChallenge;
	public bool IsBossChallenge {
		get { return isBossChallenge; }
	}

	public ImmutableDataChallenge(string id, IXMLNode xmlNode, string error) {
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		challengeType = (ChallengeTypes)Enum.Parse(typeof(ChallengeTypes), XMLUtils.GetString(hashElements["ChallengeType"] as IXMLNode, null, error));
		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
		challengeMenuSet = XMLUtils.GetString(hashElements["ChallengeMenuSet"] as IXMLNode, null, error);
		kitchenTimerMod = XMLUtils.GetFloat(hashElements["KitchenTimer"] as IXMLNode);
		customerSet = XMLUtils.GetString(hashElements["CustomerSet"] as IXMLNode);
		customerTimerMod = XMLUtils.GetFloat(hashElements["CustomerTimer"] as IXMLNode);
		dayLengthMod = XMLUtils.GetFloat(hashElements["DayLength"] as IXMLNode);
		allergy = XMLUtils.GetString(hashElements["Allergy"] as IXMLNode, null, error);
		title = XMLUtils.GetString(hashElements["Title"] as IXMLNode);
		if(hashElements.Contains("Description")) {
			challengeDescription = XMLUtils.GetString(hashElements["Description"] as IXMLNode, "", error);     // Optional
		}
		waiterMoveMod = XMLUtils.GetFloat(hashElements["WaiterMove"] as IXMLNode);
		restMode = XMLUtils.GetFloat(hashElements["RestMode"] as IXMLNode);
		custSpawnTime = XMLUtils.GetFloat(hashElements["CustomerSpawnTimer"] as IXMLNode);
		specialDecoMode = XMLUtils.GetInt(hashElements["SpecialDecoMode"] as IXMLNode);
		startingHearts = XMLUtils.GetInt(hashElements["CustomerStartingHearts"] as IXMLNode);
		if(hashElements.Contains("NextChallenge")) {
			nextChall = XMLUtils.GetString(hashElements["NextChallenge"] as IXMLNode);
		}
		if(hashElements.Contains("Bronze")) {
			bronzeBreakPoint = XMLUtils.GetInt(hashElements["Bronze"] as IXMLNode);
		}
		if(hashElements.Contains("Silver")) {
			silverBreakPoint = XMLUtils.GetInt(hashElements["Silver"] as IXMLNode);
		}
		if(hashElements.Contains("Gold")) {
			goldBreakPoint = XMLUtils.GetInt(hashElements["Gold"] as IXMLNode);
		}
		numOfTables = XMLUtils.GetInt(hashElements["Tables"] as IXMLNode);
		if(hashElements.Contains("GossiperMode")) {
			gossiperMode = XMLUtils.GetInt(hashElements["GossiperMode"]as IXMLNode);
		}
		playArea = XMLUtils.GetString(hashElements["PlayArea"] as IXMLNode);
		vipTable = XMLUtils.GetString(hashElements["Vip"] as IXMLNode);
		flyThru = XMLUtils.GetString(hashElements["Flythru"] as IXMLNode);
		isBossChallenge = XMLUtils.GetBool(hashElements["IsBossChallenge"] as IXMLNode, false);
	}
}
