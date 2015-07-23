using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

	public List <GameObject> pathNodes;

	public List<GameObject> findPath(GameObject startNode, GameObject targetNode){
		pathNodes = new List<GameObject>();
		GameObject currentNode = startNode;
		while (currentNode != targetNode){
			pathNodes.Add(currentNode);
			if(Vector2.Distance(currentNode.GetComponent<Node>().neighbors[0].transform.position,targetNode.transform.position) < Vector2.Distance(currentNode.GetComponent<Node>().neighbors[1].transform.position,targetNode.transform.position)){
				currentNode = currentNode.GetComponent<Node>().neighbors[0].gameObject;
			}
			else{
				currentNode = currentNode.GetComponent<Node>().neighbors[1].gameObject;
			}
		}
		return pathNodes;
	}
}
