﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataFood{
	private string id;
	public string ID{
		get{ return id; }
	}

	private string foodNameKey;
	public string FoodNameKey{
		get{ return foodNameKey; }
	}

	private string shortNameKey;
	public string ShortNameKey {
		get { return shortNameKey; }
	}

	private string spriteName;
	public string SpriteName{
		get{ return spriteName; }
	}

	private List<Allergies> allergyList;	// TODO NONE counts as an allergy type, need to fix
	public List<Allergies> AllergyList {
		get{ return allergyList; }
	}

	private List<FoodKeywords> keywordList;
	public List<FoodKeywords> KeywordList {
		get{ return keywordList; }
	}

	private int tier;
	public int Tier{
		get {return tier;}
	}

	public int reward;
	public int Reward{
		get {return reward;}
	}

	private int slots;
	public int Slots {
		get { return slots; }
	}

	public ImmutableDataFood(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		foodNameKey = XMLUtils.GetString(hashElements["FoodNameKey"] as IXMLNode, null, error);
		spriteName = XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode, null, error);

		// get the allergies list(optional)
		if(hashElements.ContainsKey("Allergies")){
			allergyList = new List<Allergies>();
			string strAllergies = XMLUtils.GetString(hashElements["Allergies"] as IXMLNode);
			string[] arrayAmounts = strAllergies.Split(","[0]);
			for(int i = 0; i < arrayAmounts.Length; ++i){
				allergyList.Add((Allergies)Enum.Parse(typeof(Allergies), arrayAmounts[i]));
			}
		}

		// get the short food name key(optional)
		if(hashElements.ContainsKey("FoodShortNameKey")){
			shortNameKey = XMLUtils.GetString(hashElements["FoodShortNameKey"] as IXMLNode, null, error);
		}

		// get the keywords list(optional)
		if(hashElements.ContainsKey("FoodShortNameKey")) {
			keywordList = new List<FoodKeywords>();
			string strKeywords = XMLUtils.GetString(hashElements["Keywords"] as IXMLNode);
			string[] arrayAmounts = strKeywords.Split(","[0]);
			for(int i = 0; i < arrayAmounts.Length; ++i) {
				keywordList.Add((FoodKeywords)Enum.Parse(typeof(FoodKeywords), arrayAmounts[i]));
			}
		}

		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);
		reward = XMLUtils.GetInt(hashElements["Reward"] as IXMLNode);
		slots = XMLUtils.GetInt(hashElements["Slot"] as IXMLNode);
	}
}
