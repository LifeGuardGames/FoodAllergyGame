using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EpiPenGameUiManager : MonoBehaviour{

	public List<Transform> pos;
	public void PlaceInPos(EpiPenGamePanel panel) {
		foreach(Transform spot in pos) {
			if(spot.childCount == 0) {
				panel.transform.SetParent(spot);
				//panel.transform.position = spot.position;
				panel.isCorrect = false;
				break;
			}
		}
	}
}
