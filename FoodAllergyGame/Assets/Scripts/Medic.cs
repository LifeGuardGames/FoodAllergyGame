using UnityEngine;
using System.Collections;

public class Medic : Singleton<Medic> {

	public GameObject startPos;
	public WaiterAnimController waiterAnimController;

	// the medic move toward the next customer on the list
	public void MoveToLocation(Vector3 customer){

			//If the waiter is already at its location, just call what it needs to call
			if(transform.position == customer){
				saveCustomer();
			}
			// Otherwise, move to the location and wait for callback
			else{

			//	moving = true;
//				waiterAnimController.SetMoving(true);
				
				LeanTween.cancel(gameObject);
				LeanTween.move(gameObject, customer, 0.5f)
				.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(saveCustomer);
						
		}
	}
	// if no one is sick theres no need for the medic to leave his office so he goes back
	public void MoveHome(){
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, startPos.transform.position, 0.5f)
			.setEase(LeanTweenType.easeInOutQuad);
	}
	// Stops the customer from having an allergy attack
	public void saveCustomer(){
		if(RestaurantManager.Instance.SickCustomers.Count >0){
			RestaurantManager.Instance.SickCustomers[0].GetComponent<Customer>().Saved();
		}
		else{
			MoveHome();
		}
		//RestaurantManager.Instance.SickCustomers.RemoveAt(index);
		if(	RestaurantManager.Instance.SickCustomers.Count == 0){
			MoveHome();
		}
		else{
			MoveToLocation(RestaurantManager.Instance.SickCustomers[0].transform.position);
		}
	}
}
