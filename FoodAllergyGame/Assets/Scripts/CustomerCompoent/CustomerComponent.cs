using UnityEngine;
using System.Collections;

public abstract class CustomerComponent  {

	public Customer self;
	protected int stepNum;

	// Use this for initialization
	public abstract void Reason();

	// Update is called once per frame
	public abstract void Act();
	
}
