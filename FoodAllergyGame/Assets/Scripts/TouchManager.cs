using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			RaycastHit info;
		
			if(Physics.Raycast(Input.mousePosition,Vector3.down,out info)){
				if(info.collider != null){
					switch(info.collider.gameObject.name){
					case "Table":
						if(Waiter.Instance.currentlyServing != null && info.collider.gameObject.transform.childCount == 1){	
							
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
						//	Waiter.Instance.currentlyServing
						}
						break;
					}
				}
			}
		}
	}
}
