﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenManager : MonoBehaviour {

	//CookOrder runs coroutines on orders to cook them once the coroutine is finished the food will be ready for pick up shown by 
	// MenuUIManager populates the menu sidebar for food selection

	public List<Transform> orderSpotList;

	public void CookOrder(List <GameObject> order){

		if(order.Count > 1){
			order[0].transform.SetParent(this.gameObject.transform);
			order[0].GetComponent<Order>().StartCooking();
			//order[0].SetActive(false);
			//StartCoroutine(Cooking(order[0]));
			order[1].transform.SetParent(this.gameObject.transform);
			order[1].GetComponent<Order>().StartCooking();
			//order[1].SetActive(false);
			//StartCoroutine(Cooking(order[1]));
		}
		else if(order.Count == 1){
			order[0].transform.SetParent(this.gameObject.transform);
			//StartCoroutine(Cooking(order[0]));
			order[0].GetComponent<Order>().StartCooking();
			//order[0].SetActive(false);
		}
	}

	public void Cooked(GameObject order){
		for (int i = 0; i < orderSpotList.Count; i ++){
			if(orderSpotList[i].transform.childCount == 0){
				//order.SetActive(true);
				order.transform.SetParent(orderSpotList[i].transform);
				order.transform.localPosition = new Vector3 (0,0,0);
			}
		}
	}

	public void CancelOrder(int tableNum){
		for (int i = 0; i < orderSpotList.Count; i++){
			if(orderSpotList[i].childCount > 0){
				if(orderSpotList[i].GetComponentInChildren<Order>().tableNumber == tableNum){
					//StopCoroutine(Cooking(orderSpotList[i].GetChild(0).gameObject));
					orderSpotList[i].GetChild(0).GetComponent<Order>().Canceled();
				}
			}
		}
	}
}
