using UnityEngine;
using System.Collections;

public abstract class CustomerComponent  {
	/// <summary>
	/// Each compnent contains a reference to the customer that called it and two functions an Act and Reason function
	/// Act: is the action taken when a new state is entered
	/// Reason: The logic that should be followed when transitioning to another behav
	/// The code to transition to a new state utilizes activator to create an instance of the class from the string in the xml
	/// </summary>
	public Customer self;

	// Use this for initialization
	public abstract void Reason();

	// Update is called once per frame
	public abstract void Act();
	
}
