using UnityEngine;
using System.Collections;

	public enum CustomerStates{
		None,
		InLine,
		ReadingMenu,
		WaitForOrder,
		WaitForFood,
		Eating,
		WaitForCheck,
		AllergyAttack,
		BeingRescued,
	}

	public enum WaiterHands{
		None,
		Order,
		Meal,
	}

