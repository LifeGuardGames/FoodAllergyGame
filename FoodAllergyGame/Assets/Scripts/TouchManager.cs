using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(Input.mousePosition,Vector3.down);
		if (Input.GetMouseButtonDown(0)){
			RaycastHit info;
			Debug.Log ("yo");

			if(Physics.Raycast(Input.mousePosition,Vector3.down,out info)){

				if(info.collider != null){
					switch(info.collider.gameObject.name){
					case "Table":
						if(Waiter.Instance.currentlyServing != null && info.collider.gameObject.transform.childCount == 1){	
							Waiter.Instance.currentlyServing.GetComponent<Customer>().JumpToTable(info.collider.gameObject.GetComponent<Table>().tableNumber);
						}
						else if(info.collider.gameObject.transform.childCount > 1){
							info.collider.gameObject.GetComponent<Table>().TalkToConsumer();
						}
						else{
							Waiter.Instance.MoveToLocation(info.collider.gameObject.transform.GetChild(0).position);
						}
						break;
					case "Customer":
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
