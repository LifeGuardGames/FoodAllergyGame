using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KitchenManager : MonoBehaviour {

	//CookOrder runs coroutines on orders to cook them once the coroutine is finished the food will be ready for pick up shown by 
	// MenuUIManager populates the menu sidebar for food selection

	public List<Transform> orderSpotList;

	public void CookOrder(GameObject order){
		StartCoroutine(Cooking(order));
	}

	private IEnumerator Cooking(GameObject order){
		yield return new WaitForSeconds(5.0f);
		order.GetComponent<Order>().isCooked = true;
	}
}
