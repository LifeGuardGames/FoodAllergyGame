﻿using UnityEngine;
using System.Collections;

public class Medic : Singleton<Medic> {

	public GameObject startPos;
	public WaiterAnimController waiterAnimController;

	public void MoveToLocation(Vector3 customer){

			//If the waiter is already at its location, just call what it needs to call
			if(transform.position == customer){
			Debug.Log("jvolaerbgvisdbfjvk");
				saveCustomer();
			}
			// Otherwise, move to the location and wait for callback
			else{
			Debug.Log("Location " + gameObject.transform.position + " " + customer);

			//	moving = true;
//				waiterAnimController.SetMoving(true);
				
				LeanTween.cancel(gameObject);
				LeanTween.move(gameObject, customer, 0.5f)
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
		//RestaurantManager.Instance.SickCustomers.RemoveAt(index);
		if(	RestaurantManager.Instance.SickCustomers.Count == 0){
			Debug.Log("This");
			MoveHome();
		}
		else{
			Debug.Log ("Breaking");
			MoveToLocation(RestaurantManager.Instance.SickCustomers[0].transform.position);
		}
	}
}
