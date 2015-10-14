using UnityEngine;
using System.Collections;

public class ImmutableDataTiers {

	private string id;
	public string ID{
		get { return id;}
	}

	private int tierNumber;
	public int TierNumber{
		get{ return tierNumber; }
	}

	private int cashCutoffFloor;		// If TotalCash is higher than this, then is unlocked
	public int CashCutoffFloor{
		get{ return cashCutoffFloor; }
	}

	private int menuSlots;
	public int MenuSlots{
		get{ return menuSlots; }
	}

	private string[] eventsUnlocked;
	public string[] EventsUnlocked{
		get{ return eventsUnlocked; }
	}

	private string[] foodsUnlocked;
	public string[] FoodsUnlocked{
		get{ return foodsUnlocked; }
	}

	private string[] decorationsUnlocked;
	public string[] DecorationsUnlocked{
		get{ return decorationsUnlocked; }
	}

	private string[] startArtAssets;
	public string[] StartArtAssets{
		get{ return startArtAssets; }
	}
	
	public ImmutableDataTiers(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		this.id = id;
		tierNumber = XMLUtils.GetInt(hashElements["TierNumber"] as IXMLNode);
		cashCutoffFloor = XMLUtils.GetInt(hashElements["CashCutoffFloor"] as IXMLNode);
		menuSlots = XMLUtils.GetInt(hashElements["MenuSlots"] as IXMLNode);
		eventsUnlocked = XMLUtils.GetStringList(hashElements["EventsUnlocked"] as IXMLNode);
		foodsUnlocked = XMLUtils.GetStringList(hashElements["FoodsUnlocked"] as IXMLNode);
		decorationsUnlocked = XMLUtils.GetStringList(hashElements["DecorationsUnlocked"] as IXMLNode);
		startArtAssets = XMLUtils.GetStringList(hashElements["StartArtAssets"] as IXMLNode);
	}
}
