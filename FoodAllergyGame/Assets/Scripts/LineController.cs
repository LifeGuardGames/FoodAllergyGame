using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineController : MonoBehaviour {

	public List<Transform> lineList;

	public Transform NewCustomer (){
		for (int i = 0; i < lineList.Count; i++){
			if(lineList[i].transform.childCount == 0){
				return lineList[i];
			}

		}
		return null;
	}
}

