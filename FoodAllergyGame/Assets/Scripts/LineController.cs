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

    public void FillInLine(){
        for (int i = 0; i < lineList.Count; i++) {
            if (lineList[i].childCount == 0){
                for(int j = i; j <lineList.Count; j++){
                    if (lineList[j].childCount != 0) {
                        lineList[j].transform.GetChild(0).SetParent(lineList[i].transform);
						if(lineList[i].transform.GetChild(0).position == lineList[j].transform.position) {
							lineList[i].transform.GetChild(0).position = lineList[i].transform.position;
						}
                        break;
                    }
                }
            }
        }
    }


}

