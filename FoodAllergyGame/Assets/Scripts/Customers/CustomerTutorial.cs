using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer {

	public GameObject tutFingers;
	public int step = 0;

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		satisfaction = 100;
		tutFingers= GameObject.Find("TuTFingers");
		StartCoroutine("ShowTableFinger");
	}

	public override void JumpToTable (int tableN)
	{
		hideTableFinger();
		base.JumpToTable (tableN);
	}

	public override void GetOrder ()
	{
		base.GetOrder ();
	}

	public override void OrderTaken (ImmutableDataFood food)
	{
		base.OrderTaken (food);
		StartCoroutine("ShowTuTFinger");
	}

	public override void Eating ()
	{
		hideTableFinger();
		base.Eating ();
	}

	public override void NotifyLeave ()
	{
		DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
		DataManager.Instance.GameData.RestaurantEvent.CurrentEvent = "EventT1";
		RestaurantManager.Instance.StartDay(DataLoaderEvents.GetData(DataManager.Instance.GetEvent()));
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
		yield return new WaitForSeconds(5.0f);
		Debug.Log (tutFingers.transform.GetChild(step).name);
		tutFingers.transform.GetChild(step).gameObject.SetActive(true);
	}

	IEnumerator ShowTableFinger(){
		yield return new WaitForSeconds(5.0f);
		transform.GetChild (2).gameObject.SetActive(true);
	}
}
