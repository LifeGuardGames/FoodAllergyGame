using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ImmutableDataDecoItem {

	private string id;
	public string ID{
		get{return id;}
	}

	private DecoTypes type;
	public DecoTypes Type{
		get {return type;}
	}

	private int cost;
	public int Cost{
		get {return cost;}
	}

	private string buttonTitleKey;
	public string ButtonTitleKey{
		get{return buttonTitleKey;}
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
		buttonTitleKey = XMLUtils.GetString(hashElements["TitleKey"] as IXMLNode);
		spriteName = XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode);
		if(XMLUtils.GetString(hashElements["Secondary"] as IXMLNode) != null){
			secondarySprite = XMLUtils.GetString(hashElements["Secondary"] as IXMLNode);
		}
	}
}
