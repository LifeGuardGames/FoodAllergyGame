using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchManager : Singleton<TouchManager> {
	public Queue <GameObject> inputQueue;
	private bool isPaused = false;

	void Start(){
		inputQueue = new Queue<GameObject>();
	}

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			if(!isPaused){
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
							
						// Check for other clickable objects
						IClickObject clickObject = hitObject.collider.gameObject.GetComponent<IClickObject>();
						if(clickObject != null){
							clickObject.OnClicked();
						}
					}
				}
			}
		}
	}
	public void pauseQueue(){
		isPaused = ! isPaused;
	}
//	public static bool IsHoveringOverGUI(){ TODO broken
//		if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) || 
//		   UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
//			return true;
//		}
//		else{
//			return false;
//		}
//	}
}
