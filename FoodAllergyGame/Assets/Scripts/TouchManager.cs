using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hitObject;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hitObject)){
				if(hitObject.collider != null){

					// Tap objects NEED to have implemented IWaiterSelection
					IWaiterSelection waiterSelection = hitObject.collider.gameObject.GetComponent<IWaiterSelection>();
					if(waiterSelection != null){
						waiterSelection.OnClicked();
					}
				}
			}
		}
	}
}
