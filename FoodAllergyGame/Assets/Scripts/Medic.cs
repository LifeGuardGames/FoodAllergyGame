using UnityEngine;
using System.Collections;

public class Medic : Singleton<Medic> {
	public GameObject startPos;

	public static int MedicPrice = -40;
	public static int HospitalPrice = -100;

	private int medicCost;
	public int MedicCost {
		get { return medicCost; }
	}

	private Vector3 firstCustomerPositionAux;
	private Vector3 medicOffset = new Vector3(-100f, 0f, 0f);

	public Animator medicAnimator;

	// Have the medic leave its base before moving to customers	
	public void SetOutFromHome(Vector3 customerPosition){
		AudioManager.Instance.PlayClip("MedicEnter");
		firstCustomerPositionAux = customerPosition + medicOffset;
		MoveToLocation(firstCustomerPositionAux);
	}

	// Move toward the next customer on the list
	public void MoveToLocation(Vector3 customer){
		// If the waiter is already at its location, just call what it needs to call
		if(transform.position == customer){
			SaveCustomer();
		}
	
		// Otherwise, move to the location and wait for callback
		else{
			if(!LeanTween.isTweening(this.gameObject)) {
				LeanTween.move(gameObject, customer, 1.2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(SaveCustomer);
				medicAnimator.Play("MedicFrontFlip");
			}
		}
	}

	// if no one is sick theres no need for the medic to leave his office so he goes back
	public void MoveHome(){
		medicAnimator.Play("MedicReturn");
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, startPos.transform.position, 1f).setEase(LeanTweenType.easeInOutQuad);
	}

	// Stops the customer from having an allergy attack
	public void SaveCustomer(){
		StartCoroutine("TreatCustomer");
	}

	public void BillRestaurant(int expense){
		medicCost += expense;
	}

	private IEnumerator TreatCustomer(){
		// Saved the person in time
		if(RestaurantManager.Instance.sickCustomers.Count > 0){
			ShowThoughtBubble();
			RestaurantManager.Instance.sickCustomers[0].GetComponent<Customer>().saved = true;
			RestaurantManager.Instance.sickCustomers[0].GetComponent<Customer>().StopCoroutine("AllergyTimer");
            yield return new WaitForSeconds(3.0f);
			RestaurantManager.Instance.sickCustomers[0].GetComponent<Customer>().currBehav.Reason();
		}
		// Missed the person... oh well
		else{
			HideThoughtBubble();
			MoveHome();
		}

		// Check if there are other customers that needs to be saved
		if(	RestaurantManager.Instance.sickCustomers.Count == 0){
			HideThoughtBubble();
			MoveHome();
		}
		else{
			HideThoughtBubble();
			MoveToLocation(RestaurantManager.Instance.sickCustomers[0].transform.position + medicOffset);
		}
	}

	private void ShowThoughtBubble() {
		medicAnimator.SetBool("IsThoughtOn", true);
    }

	private void HideThoughtBubble() {
		medicAnimator.SetBool("IsThoughtOn", false);
	}
}
