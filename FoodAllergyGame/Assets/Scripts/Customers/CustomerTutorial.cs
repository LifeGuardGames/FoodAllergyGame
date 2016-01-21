using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer{

	public GameObject tutFingers;
	public int step = 0;
	public bool isAllergy = false;

	public override void Init(int num, ImmutableDataEvents mode){
		base.Init(num, mode);
		satisfaction = 100;
		tutFingers = GameObject.Find("TutFingers");
		StartCoroutine("ShowTableFinger");
		if(!isAllergy){
			allergy = Allergies.None;
		}
		else{
			allergy = Allergies.Wheat;
		}
	}

	public override void OrderTaken(ImmutableDataFood food){
		hideFinger();
		base.OrderTaken(food);
		StartCoroutine("ShowTutFinger");
	}

	public override void Eating(){
		hideTableFinger();
		base.Eating();
		if(!isAllergy){
			RestaurantManager.Instance.SpawnSecondTut();
		}
	}


	public void nextHint(){
		StartCoroutine("ShowTutFinger");
	}

	public void NextTableFinger(){
		StartCoroutine("ShowTableFinger");
	}

	public void hideTableFinger(){
		transform.GetChild(2).gameObject.SetActive(false);
		StopCoroutine("ShowTableFinger");
	}

	public void hideFinger(){
		tutFingers.transform.GetChild(step).gameObject.SetActive(false);
		StopCoroutine("ShowTutFinger");
	}

	IEnumerator ShowTutFinger(){
		yield return new WaitForSeconds(0.2f);
		tutFingers.transform.GetChild(step).gameObject.SetActive(true);
	}

	IEnumerator ShowTableFinger(){
		yield return new WaitForSeconds(0.2f);
		transform.GetChild(2).gameObject.SetActive(true);
	}
}
