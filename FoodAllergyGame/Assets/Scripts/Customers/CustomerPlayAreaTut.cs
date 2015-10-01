using UnityEngine;
using System.Collections;

public class CustomerPlayAreaTut : Customer {

	public GameObject tutFingers;
	public bool usedPlayArea = false;
	// Use this for initialization
	public override void Init (int num, ImmutableDataEvents mode){
		tutFingers = GameObject.Find("TutFingers");
		transform.GetChild(2).gameObject.SetActive(true);
		base.Init(num, mode);
		satisfaction = 1;
		//UpdateSatisfaction(1);
	}
	
	public void hideTableFinger(){
		transform.GetChild(2).gameObject.SetActive(false);
	}

	public override void OnClicked ()
	{
		base.OnClicked ();
		if(!usedPlayArea){
			hideTableFinger();
			tutFingers.transform.GetChild(7).gameObject.SetActive(true);
		}
	}

	public override void GoToPlayArea (Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction)
	{
		base.GoToPlayArea (playAreaSpot, spotIndex, deltaSatisfaction);
		tutFingers.transform.GetChild(7).gameObject.SetActive(false);
		usedPlayArea = true;
	}
}
