﻿using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(Input.mousePosition,Vector3.right);
		if (Input.GetMouseButtonDown(0)){
			RaycastHit info;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out info)){
				Debug.Log (info.collider.gameObject.name);
				if(info.collider != null){
					switch(info.collider.gameObject.tag){
					case "Table":
						if(Waiter.Instance.currentlyServing != null && !info.collider.gameObject.GetComponent<Table>().inUse){	
							Waiter.Instance.currentlyServing.GetComponent<Customer>().JumpToTable(info.collider.gameObject.GetComponent<Table>().tableNumber);
							info.collider.gameObject.GetComponent<Table>().inUse = true;
						}
						else if(info.collider.gameObject.transform.childCount > 1){
							Debug.Log ("Hello");
							Waiter.Instance.MoveToLocation(info.collider.gameObject.transform.GetChild(2).position);
							info.collider.gameObject.GetComponent<Table>().TalkToConsumer();
						}
						else{
							Debug.Log ("Walking");
							Waiter.Instance.MoveToLocation(info.collider.gameObject.transform.GetChild(2).position);
						}
						break;
					case "Customer":
						Debug.Log ("Customer");
						if(info.collider.gameObject.GetComponent<Customer>().state == CustomerStates.InLine){
							Waiter.Instance.currentlyServing = info.collider.gameObject;
						}
						break;
					case "Kitchen":
						Waiter.Instance.MoveToLocation(info.collider.gameObject.transform.GetChild(0).position);
						info.collider.gameObject.GetComponent<KitchenManager>().CookOrder(Waiter.Instance.OrderChef());
						break;
					case "Order":
						Waiter.Instance.SetHand(info.collider.gameObject);
						break;
					}
				}
			}
		}
	}
}
