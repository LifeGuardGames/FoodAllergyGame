using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit info;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out info)){
				if(info.collider != null){
					switch(info.collider.gameObject.tag){
					case "Table":
						if(Waiter.Instance.currentLineCustomer != null && !info.collider.gameObject.GetComponent<Table>().inUse){	
							Waiter.Instance.currentLineCustomer.transform.localScale = Vector3.one;
							Waiter.Instance.currentLineCustomer.GetComponent<Customer>().JumpToTable(info.collider.gameObject.GetComponent<Table>().tableNumber);
							info.collider.gameObject.GetComponent<Table>().inUse = true;
						}
						else if(info.collider.gameObject.transform.childCount > 1){
							Waiter.Instance.MoveToLocation(info.collider.gameObject.transform.GetChild(2).position);
							info.collider.gameObject.GetComponent<Table>().TalkToConsumer();
						}
						else{
							Waiter.Instance.MoveToLocation(info.collider.gameObject.transform.GetChild(2).position);
						}
						break;
					case "Customer":
						if(info.collider.gameObject.GetComponent<Customer>().state == CustomerStates.InLine){
							if(Waiter.Instance.CheckHands()){
								// If you were already selecting a customer, untween that
								if(Waiter.Instance.currentLineCustomer != null){
									Customer customerScript = Waiter.Instance.currentLineCustomer.GetComponent<Customer>();
									if(customerScript != null){
										if(customerScript.state == CustomerStates.InLine){
											customerScript.transform.localScale = Vector3.one;
										}
									}
								}
								Waiter.Instance.currentLineCustomer = info.collider.gameObject;
								Waiter.Instance.currentLineCustomer.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
							}
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
