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

	private string titleKey;
	public string TitleKey{
		get{return titleKey;}
	}

	private string descriptionKey;
	public string DescriptionKey{
		get{return descriptionKey;}
	}

	private string spriteName;
	public string SpriteName{
		get { return spriteName;}
	}

	private int tier;
	public int Tier{
		get{return tier;}
	}



	private int iapPrice;
	public int IapPrice {
		get { return iapPrice; }
	}

	private DecoTypes iapType;
	public DecoTypes IapType {
		get { return iapType; }
	}

	public ImmutableDataDecoItem(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);

		this.id = id;
		type = (DecoTypes)Enum.Parse(typeof(DecoTypes), XMLUtils.GetString(hashElements["Type"] as IXMLNode));
		cost = XMLUtils.GetInt(hashElements["Price"] as IXMLNode);
		titleKey = XMLUtils.GetString(hashElements["TitleKey"] as IXMLNode);
		descriptionKey = XMLUtils.GetString(hashElements["DescriptionKey"] as IXMLNode);

		if(XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode) != null){
			spriteName = XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode);
		}

		tier = XMLUtils.GetInt(hashElements["Tier"] as IXMLNode);

		if(hashElements.Contains("IapPrice")) {
			iapPrice = XMLUtils.GetInt(hashElements["IapPrice"] as IXMLNode);
		}
		else {
			iapPrice = 0;
		}
		if(hashElements.Contains("IapType")) {
			iapType = (DecoTypes)Enum.Parse(typeof(DecoTypes), XMLUtils.GetString(hashElements["IapType"] as IXMLNode));
		}
	}
}
