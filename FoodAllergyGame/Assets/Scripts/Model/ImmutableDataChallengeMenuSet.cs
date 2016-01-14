using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataChallengeMenuSet{

	private string id;
	public string ID{
		get { return id;}
	}

	private string[] challengeMenuSet;
	public string[] ChallengeMenuSet {
		get{ return challengeMenuSet; }
	}

	public ImmutableDataChallengeMenuSet(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		challengeMenuSet = XMLUtils.GetStringList(hashElements["FoodList"]as IXMLNode);
	}
}
