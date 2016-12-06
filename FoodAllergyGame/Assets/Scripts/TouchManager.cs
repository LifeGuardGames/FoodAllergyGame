using UnityEngine;
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
							waiterSelection.OnPressAnim();	// Call press animations if exists
                            if(!waiterSelection.IsQueueable()){
								waiterSelection.OnClicked();
							}
							else{
								if(inputQueue.Count < queueLimit){
									inputQueue.Enqueue(hitObject.collider.gameObject);

									// Add queue mark
									waiterSelection.AddQueueUI();
									RefreshQueueUI();

									// used to dequeue
									if(Waiter.Instance.CanMove){
										Waiter.Instance.Finished();
									}
								}
							}
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

	public void EmptyQueue(){
		inputQueue = new Queue<GameObject>();
	}

	// During dequeue, refresh update the status of all the current elements in the UI
	public void RefreshQueueUI() {
		int queueOrder = 0;
		foreach(GameObject go in inputQueue) {
			IWaiterSelection waiterSelection = go.GetComponent<IWaiterSelection>();
			if(waiterSelection != null) {
				waiterSelection.UpdateQueueUI(queueOrder);
				queueOrder++;
			}
			else {
				Debug.LogError("Waiter selection now found");
			}
		}
	}
}
