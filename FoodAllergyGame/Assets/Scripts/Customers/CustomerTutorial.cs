using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer {

	public GameObject tutFingers;
	public int step = 0;
	public bool isAllergy = false;

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		satisfaction = 100;
		tutFingers= GameObject.Find("TuTFingers");
		StartCoroutine("ShowTableFinger");
		if(!isAllergy){
			allergy = Allergies.None;
		}
		else{
			allergy = Allergies.Wheat;
		}
	}

	public override void JumpToTable (int tableN)
	{
		hideFinger();
		base.JumpToTable (tableN);
	}

	public override void GetOrder ()
	{
		base.GetOrder ();
	}

	public override void OrderTaken (ImmutableDataFood food)
	{
		hideFinger();
		base.OrderTaken (food);
		StartCoroutine("ShowTuTFinger");
	}

	public override void Eating ()
	{
		hideTableFinger();
		base.Eating ();
		if(!isAllergy){
			RestaurantManager.Instance.SpawnSecondTut();
		}
	}

	public override void NotifyLeave ()
	{
		base.NotifyLeave ();
	}

	public void nextHint(){
		StartCoroutine("ShowTuTFinger");
	}

	public void NextTableFinger(){
		StartCoroutine("ShowTableFinger");
	}

	public void hideTableFinger(){
		transform.GetChild (2).gameObject.SetActive(false);
		StopCoroutine("ShowTableFinger");
	}

	public void hideFinger(){
		tutFingers.transform.GetChild(step).gameObject.SetActive(false);
		StopCoroutine("ShowTuTFinger");
	}

	IEnumerator ShowTuTFinger(){
		yield return new WaitForSeconds(2.0f);
		Debug.Log (tutFingers.transform.GetChild(step).name);
		tutFingers.transform.GetChild(step).gameObject.SetActive(true);
	}

	IEnumerator ShowTableFinger(){
		yield return new WaitForSeconds(2.0f);
		transform.GetChild (2).gameObject.SetActive(true);
	}
}
