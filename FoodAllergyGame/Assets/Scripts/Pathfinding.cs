using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

	public int bestCost;
	public GameObject targetNode;
	public List<Node> openList;
	public List<Node> closedList;
	

////	public List<GameObject> FindPath(GameObject target, GameObject currNode){
//		for (int i = 0; i < currNode.GetComponent<Node>().neighbors.Length; i ++){
//			if(!openList.Contains(currNode.GetComponent<Node>().neighbors[i])){
//				openList.Add(currNode.GetComponent<Node>().neighbors[i]);
//				currNode.GetComponent<Node>().neighbors[i].distFromHome += (currNode.GetComponent<Node>().distFromHome + 1);
//				currno
//			}
//		}
//	}
}
