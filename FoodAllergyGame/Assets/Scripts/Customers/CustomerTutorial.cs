using UnityEngine;
using System.Collections;

public class CustomerTutorial : Customer{

	public GameObject tutFingers;
	public GameObject ownFinger;
	public int step = 0;
	public bool isAllergy = false;

	public override void Init(int num, ImmutableDataChallenge mode){
		
		satisfaction = 100;
		tutFingers = GameObject.Find("TutFingers");
		StartCoroutine("ShowTableFinger");
		base.Init(num, mode);
		if(!isAllergy) {
			AnalyticsManager.Instance.DayOneFunnel("2 Customer Start");
			allergy[0] = Allergies.None;
			RestaurantManager.Instance.GetTable(1).inUse = true;
			RestaurantManager.Instance.GetTable(2).inUse = true;
			RestaurantManager.Instance.GetTable(3).inUse = true;
			RestaurantManager.Instance.GetTable(4).inUse = true;
		}
		else {
			allergy[0] = Allergies.Peanut;
			menuTimer *= 2;
		}
		
	}

	public override void OrderTaken(ImmutableDataFood food){
		hideFinger();
		base.OrderTaken(food);
		StartCoroutine("ShowTutFinger");
		transform.GetChild(3).gameObject.SetActive(false);
	}

	public override void Eating(){
		hideTableFinger();
		base.Eating();
		transform.GetChild(3).gameObject.SetActive(false);
		if(!isAllergy){
			RestaurantManagerChallenge.Instance.GetComponent<RestaurantManagerChallenge>().SpawnSecondTut();
		}
	}


	public void nextHint(){
		StartCoroutine("ShowTutFinger");
	}

	public void NextTableFinger(){
		StartCoroutine("ShowTableFinger");
	}

	public void hideTableFinger(){
		ownFinger.SetActive(false);
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
		transform.GetChild(3).gameObject.SetActive(true);
	}
}
