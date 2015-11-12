using UnityEngine;
using System.Collections;

public class Medic : Singleton<Medic> {

	public GameObject startPos;
	public int medicCost;
	public int MedicCost{
		get{return medicCost;}
	}
	public WaiterAnimController waiterAnimController;

	private Vector3 firstCustomerPositionAux;

	// Have the medic leave its base before moving to customers	
	public void SetOutFromHome(Vector3 customerPosition){
		AudioManager.Instance.PlayClip("MedicEnter");
		firstCustomerPositionAux = customerPosition;
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, new Vector3(transform.position.x + 300f, transform.position.y, transform.position.z), 0.5f)
			.setOnComplete(OnSetOutFromHomeFinished);
	}

	private void OnSetOutFromHomeFinished(){
		MoveToLocation(firstCustomerPositionAux);
	}

	// Move toward the next customer on the list
	public void MoveToLocation(Vector3 customer){
		//If the waiter is already at its location, just call what it needs to call
		if(transform.position == customer){
			SaveCustomer();
		}
		// Otherwise, move to the location and wait for callback
		else{
			LeanTween.cancel(gameObject);
			LeanTween.move(gameObject, customer, 0.5f)
			.setEase(LeanTweenType.easeInOutQuad)
				.setOnComplete(SaveCustomer);
		}
	}
	// if no one is sick theres no need for the medic to leave his office so he goes back
	public void MoveHome(){
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, startPos.transform.position, 0.5f)
			.setEase(LeanTweenType.easeInOutQuad);
	}

	// Stops the customer from having an allergy attack
	public void SaveCustomer(){
		StartCoroutine("TreatCustomer");

	}

	public void BillRestaurant (int expense){
		medicCost += expense;
	}

	private IEnumerator TreatCustomer(){
		yield return new WaitForSeconds(1.0f);

		// Saved the person in time
		if(RestaurantManager.Instance.SickCustomers.Count > 0){
			RestaurantManager.Instance.SickCustomers[0].GetComponent<Customer>().Saved();
		}
		// Missed the person... oh well
		else{
			MoveHome();
		}

		// Check if there are other customers that needs to be saved
		if(	RestaurantManager.Instance.SickCustomers.Count == 0){
			MoveHome();
		}
		else{
			MoveToLocation(RestaurantManager.Instance.SickCustomers[0].transform.position);
		}
	}
}
