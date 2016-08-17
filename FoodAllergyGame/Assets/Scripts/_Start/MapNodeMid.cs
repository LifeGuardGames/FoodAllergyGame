using UnityEngine;
using System.Collections.Generic;

public class MapNodeMid : MonoBehaviour {

	public void Init(ImmutableDataTiers tier, int index, List<Vector2> positionList) {
		transform.localPosition = new Vector3(positionList[index].x, positionList[index].y);
	}
}
