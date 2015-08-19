using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ImmutableDataDecoItem {

	public string id;
	public string ID{
		get{return id;}
	}

	public DecoTypes type;
	public DecoTypes Type{
		get {return type;}
	}

	private int cost;
	public int Cost{
		get {return cost;}
	}

	private string buttonTitle;
	public string ButtonTitle{
		get{return buttonTitle;}
	}

	private string spriteName;
	public string SpriteName{
		get { return spriteName;}
	}

	private string secondarySprite;
	public string SecondarySprite{
		get {return secondarySprite;}
	}


	public ImmutableDataDecoItem(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		type = (DecoTypes)Enum.Parse(typeof(DecoTypes), XMLUtils.GetString(hashElements["Type"] as IXMLNode));
		cost = XMLUtils.GetInt(hashElements["Price"] as IXMLNode);
		buttonTitle = XMLUtils.GetString(hashElements["Title"] as IXMLNode);
		spriteName = XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode);
		if(XMLUtils.GetString(hashElements["Secondary"] as IXMLNode) != null){
			secondarySprite = XMLUtils.GetString(hashElements["Secondary"] as IXMLNode);
		}
	}
}
