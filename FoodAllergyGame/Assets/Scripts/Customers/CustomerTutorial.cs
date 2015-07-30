using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer {

	private GameObject tutFingers;
	private int step = 0;

	public override void Init (int num, ImmutableDataEvents mode)
	{
		base.Init (num, mode);
		satisfaction = 100;
		tutFingers= GameObject.Find("TutHints");
		StartCoroutine("ShowTuTFinger");
	}

	public override void JumpToTable (int tableN)
	{
		hideFinger();
		StopCoroutine("ShowTuTFinger");
		step++;
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
		base.NotifyLeave ();
	}

	public void nextHint(){
		StartCoroutine("ShowTuTFinger");
	}

	public void hideFinger(){
		tutFingers.transform.GetChild(step-1).gameObject.SetActive(false);
	}

	IEnumerator ShowTuTFinger(){
		yield return new WaitForSeconds(5.0f);
		if(step == 0){
			tutFingers.transform.GetChild(step).SetParent(this.gameObject.transform);
		}
		tutFingers.transform.GetChild(step).gameObject.SetActive(true);
		step++;
	}
}
