using UnityEngine;
using System.Collections;

public class Medic : Singleton<Medic> {

	public GameObject startPos;
	public WaiterAnimController waiterAnimController;

	public void MoveToLocation(GameObject customer){
		Debug.Log("jvolaerbgvisdbfjvk");
			//If the waiter is already at its location, just call what it needs to call
			if(transform.position == customer.transform.position){
				saveCustomer();
			}
			// Otherwise, move to the location and wait for callback
			else{
			//	moving = true;
			//	waiterAnimController.SetMoving(true);
				
				LeanTween.cancel(gameObject);
				LeanTween.move(gameObject, customer.transform.position, 0.5f)
				.setEase(LeanTweenType.easeInOutQuad)
					.setOnComplete(saveCustomer);
						
		}
	}

	public void MoveHome(){
		LeanTween.cancel(gameObject);
		LeanTween.move(gameObject, startPos.transform.position, 0.5f)
			.setEase(LeanTweenType.easeInOutQuad);
	}

	public void saveCustomer(){
		RestaurantManager.Instance.SickCustomers[0].GetComponent<Customer>().Saved();
	}
}
