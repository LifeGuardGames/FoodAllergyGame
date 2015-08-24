using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : Singleton<Pathfinding> {

	public List <GameObject> pathNodes;
	public bool showPath = true;

	public List<GameObject> findPath(GameObject startNode, GameObject targetNode){
		pathNodes = new List<GameObject>();
		GameObject currentNode = startNode;
		float dist = 100000f;
		GameObject tempNode;
		tempNode = currentNode;
		while (currentNode != targetNode){
			for (int i = 0; i < currentNode.GetComponent<Node>().neighbors.Length;i++){
				if(Vector2.Distance(currentNode.GetComponent<Node>().neighbors[i].transform.position,targetNode.transform.position) <= dist){
					dist = Vector2.Distance(currentNode.GetComponent<Node>().neighbors[i].transform.position,targetNode.transform.position);
					tempNode = currentNode.GetComponent<Node>().neighbors[i].gameObject;
				}
				else{
//					Debug.LogError("Too far");
				}
			}
			currentNode = tempNode;
			pathNodes.Add(currentNode);
			dist = 100000f;
		}
		pathNodes.Add(targetNode);
		return pathNodes;
	}
}
