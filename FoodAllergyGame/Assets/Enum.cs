using UnityEngine;
using System.Collections;

public class Enum : MonoBehaviour {
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
