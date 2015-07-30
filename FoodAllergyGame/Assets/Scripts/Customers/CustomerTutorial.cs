using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer {

	private GameObject tutFingers;
	public int step = 0;

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		satisfaction = 100;
		tutFingers= GameObject.Find("TutHints");
		StartCoroutine("ShowTuTFinger");
	}

	public override void JumpToTable (int tableN)
	{
		hideTableFinger();
		StopCoroutine("ShowTuTFinger");
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
		transform.GetChild (2).gameObject.SetActive(true);
	}

	public void hideFinger(){
		tutFingers.transform.GetChild(step).gameObject.SetActive(false);
	}

	IEnumerator ShowTuTFinger(){
		yield return new WaitForSeconds(5.0f);
		tutFingers.transform.GetChild(step).gameObject.SetActive(true);
	}

	IEnumerator ShowTableFinger(){
		yield return new WaitForSeconds(5.0f);
		transform.GetChild (2).gameObject.SetActive(true);
	}
}
