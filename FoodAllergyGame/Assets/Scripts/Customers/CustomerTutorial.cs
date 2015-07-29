using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer {

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		satisfaction = 100;
	}
	public override void JumpToTable (int tableN)
	{
		base.JumpToTable (tableN);
	}
	public override void GetOrder ()
	{
		base.GetOrder ();
	}
	public override void OrderTaken (ImmutableDataFood food)
	{
		base.OrderTaken (food);
	}
	public override void NotifyLeave ()
	{
		base.NotifyLeave ();
	}
}
