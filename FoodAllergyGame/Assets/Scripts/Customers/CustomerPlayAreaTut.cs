using UnityEngine;
using System.Collections;

public class CustomerPlayAreaTut : Customer {

	public GameObject tutFingers;
	//public bool usedPlayArea = false;
	// Use this for initialization
	public override void Init (int num, ImmutableDataChallenge mode){
		tutFingers = GameObject.Find("TutFingers");
		transform.GetChild(2).gameObject.SetActive(true);
		base.Init(num, mode);
		//satisfaction = 1;
		StopCoroutine("SatisfactionTimer");
		UpdateSatisfaction(-2);
	}


	public void hideTableFinger(){
		transform.GetChild(2).gameObject.SetActive(false);
	}

	public override void OnClicked (){
		base.OnClicked ();
		//if(!usedPlayArea){
			hideTableFinger();
		if(!RestaurantManager.Instance.IsTableAvailable()){
			tutFingers.transform.GetChild(7).gameObject.SetActive(true);
		}
		//}
	}

	public override void GoToPlayArea (Vector3 playAreaSpot, int spotIndex, int deltaSatisfaction){
		base.GoToPlayArea (playAreaSpot, spotIndex, deltaSatisfaction);
		tutFingers.transform.GetChild(7).gameObject.SetActive(false);
		//usedPlayArea = true;
		transform.GetChild(2).gameObject.SetActive(false);
		StartCoroutine("ShowFinger");
	}

	IEnumerator ShowFinger(){
		yield return new WaitForSeconds(10.0f);
		transform.GetChild(2).gameObject.SetActive(true);
		StopCoroutine("SatisfactionTimer");
	}

	public override void Deselect() {
		base.Deselect();
		tutFingers.transform.GetChild(7).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(true);

	}
}
