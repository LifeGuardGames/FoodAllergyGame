using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataStartObjects{

	private string id;
	public string ID{
		get{ return id; }
	}

	private string spriteName;
	public string SpriteName{
		get{return spriteName;}
	}

	private string animationKey;
	public string AnimationKey{
		get{return animationKey;}
	}

	public ImmutableDataStartObjects(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		spriteName = XMLUtils.GetString(hashElements["SpriteName"] as IXMLNode, null, error);
		animationKey = XMLUtils.GetString(hashElements["AnimationKey"] as IXMLNode, null, error);
	}
}
