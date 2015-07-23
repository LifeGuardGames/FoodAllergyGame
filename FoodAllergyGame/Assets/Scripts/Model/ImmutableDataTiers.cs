using UnityEngine;
using System.Collections;

public class ImmutableDataTiers : MonoBehaviour {

	private string id;
	public string ID{
		get { return id;}
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

		menuSlots = XMLUtils.GetInt(hashElements["MenuSlots"] as IXMLNode);
		eventsUnlocked = XMLUtils.GetStringList(hashElements["EventsUnlocked"] as IXMLNode);
		foodsUnlocked = XMLUtils.GetStringList(hashElements["FoodsUnlocked"] as IXMLNode);
		decorationsUnlocked = XMLUtils.GetStringList(hashElements["DecorationsUnlocked"] as IXMLNode);
		startArtAssets = XMLUtils.GetStringList(hashElements["StartArtAssets"] as IXMLNode);
	}
}
