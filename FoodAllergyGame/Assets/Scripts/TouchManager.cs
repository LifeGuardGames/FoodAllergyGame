using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchManager : Singleton<TouchManager> {
	public Queue <GameObject> inputQueue;
	void Start(){
		inputQueue = new Queue <GameObject>();
	}
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hitObject;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hitObject)){
				if(hitObject.collider != null){

					// Tap objects NEED to have implemented IWaiterSelection
					IWaiterSelection waiterSelection = hitObject.collider.gameObject.GetComponent<IWaiterSelection>();
					if(waiterSelection != null){
						inputQueue.Enqueue(hitObject.collider.gameObject);
						if(Waiter.Instance.canMove){
							Waiter.Instance.Finished();
						}
					}
				}
			}
		}
	}
}
