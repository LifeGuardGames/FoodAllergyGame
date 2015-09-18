using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchManager : Singleton<TouchManager> {
	public Queue <GameObject> inputQueue;
	public int queueLimit = 6;
	public bool isPaused = false;

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
							if(!waiterSelection.IsQueueable()){
								waiterSelection.OnClicked();
							}
							else{
								if(inputQueue.Count < queueLimit){
									inputQueue.Enqueue(hitObject.collider.gameObject);
									// used to dequeue
									if(Waiter.Instance.CanMove){
										Waiter.Instance.Finished();
									}
								}
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
	public void PauseQueue(){
		isPaused = true;
	}

	public void UnpauseQueue(){
		isPaused = false;
	}
}
