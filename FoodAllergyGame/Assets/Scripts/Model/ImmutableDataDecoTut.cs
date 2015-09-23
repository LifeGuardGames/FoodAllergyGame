using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImmutableDataDecoTut{

	private string id;
	public string ID{
		get{ return id; }
	}

	private string image;
	public string Image{
		get{return image;}
	}

	private string imageMid;
	public string ImageMid{
		get{return imageMid;}
	}

	private string imageEnd;
	public string ImageEnd{
		get{return imageEnd;}
	}

	private string text;
	public string Text{
		get { return text;}
	}

	public ImmutableDataDecoTut(string id, IXMLNode xmlNode, string error){
		Hashtable hashElements = XMLUtils.GetChildren(xmlNode);
		
		this.id = id;
		image = XMLUtils.GetString(hashElements["Image"] as IXMLNode, null, error);
		imageMid = XMLUtils.GetString(hashElements["ImageMid"] as IXMLNode, null, error);
		imageEnd = XMLUtils.GetString(hashElements["ImageEnd"] as IXMLNode, null, error);
		text = XMLUtils.GetString(hashElements["Text"] as IXMLNode);
	}
}
