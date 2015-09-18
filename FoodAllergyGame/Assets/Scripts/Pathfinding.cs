using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : Singleton<Pathfinding> {
	public bool showPath = true;	// Used for gizmos

	public GameObject nodeFlyThru;
	public GameObject NodeFlyThru{
		get{ return nodeFlyThru; }
	}

	public GameObject nodeMedic;
	public GameObject NodeMedic{
		get{ return nodeMedic; }
	}

	public GameObject nodeVIP;
	public GameObject NodeVIP{
		get{ return nodeVIP; 	}
	}

	public GameObject nodeBathroom;
	public GameObject NodeBathroom{
		get{ return nodeBathroom; }
	}

	public GameObject nodeKitchen;
	public GameObject NodeKitchen{
		get{ return nodeKitchen; }
	}

	public GameObject nodeMicrowave;
	public GameObject NodeMicrowave{
		get{ return nodeMicrowave; }
	}

	public GameObject nodeSpinner;
	public GameObject NodeSpinner{
		get{ return nodeSpinner; }
	}

	public List<GameObject> nodeTableList;			// Only for normal tables, not VIP/Flythru
	public GameObject GetNormalTableNode(int index){
		return nodeTableList[index];
	}

	public List<GameObject> FindPath(GameObject startNode, GameObject targetNode){
		List <GameObject> pathNodes = new List<GameObject>();
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
