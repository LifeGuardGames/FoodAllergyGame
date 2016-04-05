using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public Node[] neighbors;

	public int baseSortingOrder;
	public int BaseSortingOrder {
		get { return baseSortingOrder; }
	}

#if UNITY_EDITOR
	void OnDrawGizmos(){
		if(Pathfinding.Instance.showPath){
			for (int i = 0; i < neighbors.Length;i++){
				Gizmos.color = Color.green;
				Gizmos.DrawCube(this.gameObject.transform.position, new Vector3 (0.5f,0.5f,0.5f));
				Gizmos.DrawLine(this.gameObject.transform.position, neighbors[i].gameObject.transform.position);
			}
		}
	}
#endif
}
