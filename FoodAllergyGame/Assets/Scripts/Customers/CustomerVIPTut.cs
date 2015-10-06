﻿using UnityEngine;
using System.Collections;

public class CustomerVIPTut : Customer {

	public GameObject tutFingers;
	// Use this for initialization
	public override void Init (int num, ImmutableDataEvents mode){
		tutFingers = GameObject.Find("TutFingers");
		showCustomerFinger();
		base.Init(num, mode);
	}

	public void showCustomerFinger(){
		transform.GetChild(2).gameObject.SetActive(true);
	}

	public void hideCustomerFinger(){
		transform.GetChild(2).gameObject.SetActive(false);
	}
	
	public override void OnClicked ()
	{
		base.OnClicked ();
		hideCustomerFinger();
		tutFingers.transform.GetChild(8).gameObject.SetActive(true);
	}


}