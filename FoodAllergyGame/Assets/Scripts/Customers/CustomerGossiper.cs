using UnityEngine;
using System.Collections;

public class CustomerGossiper : Customer{

	public override void OrderTaken (ImmutableDataFood food)
	{
		base.OrderTaken (food);

	}
}
