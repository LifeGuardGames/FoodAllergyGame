﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataFood : MonoBehaviour {
	private string id;
	public string ID{
		get{ return id; }
	}

	private string foodNameKey;
	public string FoodNameKey{
		get{ return foodNameKey; }
	}

	private List<Allergies> allergyList;
	public List<Allergies> AllergyList {
		get{ return AllergyList; }
	}

	private List<FoodKeywords> keywordList;
	public List<FoodKeywords> KeywordList {
		get{ return KeywordList; }
	}
	
	public ImmutableDataFood(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		foodNameKey = XMLUtils.GetString(hashElements["FoodNameKey"] as IXMLNode, null, error);

		// get the allergies list(optional)
		if(hashElements.ContainsKey("Allergies")){
			allergyList = new List<Allergies>();
			string strAllergies = XMLUtils.GetString(hashElements["Allergies"] as IXMLNode);
			string[] arrayAmounts = strAllergies.Split(","[0]);
			for(int i = 0; i < arrayAmounts.Length; ++i){
				allergyList.Add((Allergies)Enum.Parse(typeof(Allergies), arrayAmounts[i]));
			}
		}

		// get the keywords list(optional)
		if(hashElements.ContainsKey("Keywords")){
			keywordList = new List<FoodKeywords>();
			string strKeywords = XMLUtils.GetString(hashElements["Keywords"] as IXMLNode);
			string[] arrayAmounts = strKeywords.Split(","[0]);
			for(int i = 0; i < arrayAmounts.Length; ++i){
				keywordList.Add((FoodKeywords)Enum.Parse(typeof(FoodKeywords), arrayAmounts[i]));
			}
		}
	}
}