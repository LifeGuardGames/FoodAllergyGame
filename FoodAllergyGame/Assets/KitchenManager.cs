using UnityEngine;
using System.Collections;

public class KitchenManager : MonoBehaviour {

	//CookOrder runs coroutines on orders to cook them once the coroutine is finished the food will be ready for pick up shown by 
	// MenuUIManager populates the menu sidebar for food selection

	public void CookOrder(GameObject od){
		StartCoroutine(Cooking(od));
	}

	IEnumerator Cooking(GameObject _order){
		yield return new WaitForSeconds(5.0f);
		_order.GetComponent<Order>().isCooked = true;
	}

}
