using UnityEngine;
using System.Collections;

public class CustomerVIPTut : Customer {

	public GameObject tutFingers;
	public int tutNumber = 0;
	// Use this for initialization
	public override void Init (int num, ImmutableDataChallenge mode){
		tutFingers = GameObject.Find("TutFingers");
		ShowCustomerFinger();
		base.Init(num, mode);
	}

	public void ShowCustomerFinger(){
		transform.GetChild(3).gameObject.SetActive(true);
	}

	public void HideCustomerFinger(){
		transform.GetChild(3).gameObject.SetActive(false);
	}

	public override void OnClicked () {
		base.OnClicked ();
		HideCustomerFinger();
		tutFingers.transform.GetChild(tutNumber).gameObject.SetActive(true);
	}

	public void HideTableFinger() {
		tutFingers.transform.GetChild(tutNumber).gameObject.SetActive(false);
	}
	public override void Deselect() {
		ShowCustomerFinger();
		tutFingers.transform.GetChild(tutNumber).gameObject.SetActive(false);
		base.Deselect();
	}

}
