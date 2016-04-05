using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineController : MonoBehaviour {

	public List<Transform> lineList;
	public List<int> lineBaseSortingOrders;

	public bool HasEmptySpot() {
		for(int i = 0; i < lineList.Count; i++) {
			if(lineList[i].transform.childCount == 0) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// INVARIABLE: Line has an empty spot to begin with
	/// </summary>
	public void PlaceCustomerInEmptySpot(Customer customerScript) {
		for(int i = 0; i < lineList.Count; i++) {
			if(lineList[i].transform.childCount == 0) {
				customerScript.transform.SetParent(lineList[i]);
				customerScript.SetBaseSortingOrder(GetLineSpotBaseSortingOrder(i)); // Remember to update the customer's sorting order too
				return;
			}
		}
		Debug.LogError("No space in line, but still setting");
	}

    public void FillInLine(){
        for(int i = 0; i < lineList.Count; i++) {
            if(lineList[i].childCount == 0) {
                for(int j = i; j < lineList.Count; j++) {
                    if(lineList[j].childCount != 0) {
						Customer customerScript = lineList[j].transform.GetChild(0).GetComponent<Customer>();
						customerScript.transform.SetParent(lineList[i]);
						customerScript.SetBaseSortingOrder(GetLineSpotBaseSortingOrder(i));

						if(lineList[i].transform.GetChild(0).position == lineList[j].position) {
							lineList[i].transform.GetChild(0).position = lineList[i].position;
						}
                        break;
                    }
                }
            }
        }
    }

	private int GetLineSpotBaseSortingOrder(int lineIndex) {
		return lineBaseSortingOrders[lineIndex];
	}
}

