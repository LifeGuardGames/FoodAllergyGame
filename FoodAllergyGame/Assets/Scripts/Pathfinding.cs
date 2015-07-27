using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : Singleton<Pathfinding> {

	public List <GameObject> pathNodes;
	public GameObject testNode1;
	public GameObject testNode2;


	void Start(){

	}

	public List<GameObject> findPath(GameObject startNode, GameObject targetNode){
		Debug.Log ("Do We Get Here");
		pathNodes = new List<GameObject>();
		GameObject currentNode = startNode;
		Debug.Log ("or here");
		float dist = 100000f;
		GameObject tempNode;
		tempNode = currentNode;
		while (currentNode != targetNode){
			Debug.Log (currentNode.name);
			pathNodes.Add(currentNode);
			for (int i = 0; i < currentNode.GetComponent<Node>().neighbors.Length;i++){
				if(Vector2.Distance(currentNode.GetComponent<Node>().neighbors[i].transform.position,targetNode.transform.position) <= dist){
					dist = Vector2.Distance(currentNode.GetComponent<Node>().neighbors[i].transform.position,targetNode.transform.position);
					tempNode = currentNode.GetComponent<Node>().neighbors[i].gameObject;
				}
			}
			currentNode = tempNode;
		}
		pathNodes.Add(targetNode);
		return pathNodes;
	}
}
