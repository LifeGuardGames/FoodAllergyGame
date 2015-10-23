using UnityEngine;
using System.Collections;

public class CustomerVIPTut : Customer {

	public GameObject tutFingers;
	public int tutNumber = 0;
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

	public override void JumpToTable(int _tableNum) {
		base.JumpToTable(_tableNum);
		tutFingers.transform.GetChild(tutNumber).gameObject.SetActive(false);
		for(int i = 0; i < 4; i++) {
			RestaurantManager.Instance.GetTable(i).gameObject.GetComponent<BoxCollider>().enabled = true;
		}
	}

	public override void OnClicked ()
	{
		base.OnClicked ();
		hideCustomerFinger();
		tutFingers.transform.GetChild(tutNumber).gameObject.SetActive(true);
	}
}
