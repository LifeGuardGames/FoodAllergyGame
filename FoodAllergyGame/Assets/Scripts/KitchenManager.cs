using UnityEngine;
using System.Collections;

public class KitchenManager : MonoBehaviour {

	//CookOrder runs coroutines on orders to cook them once the coroutine is finished the food will be ready for pick up shown by 
	// MenuUIManager populates the menu sidebar for food selection

	public void CookOrder(Order order){
		StartCoroutine(Cooking(order));
	}

	private IEnumerator Cooking(Order order){
		yield return new WaitForSeconds(5.0f);
		order.isCooked = true;
	}
}
