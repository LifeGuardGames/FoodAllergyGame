using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapNodeMid : MapNode{
	public Animation reachedAnim;
	public Color activeColor;
	public Image nodeImage;

	public void Init(ImmutableDataTiers tier, int index, List<Vector2> positionList) {
		transform.localPosition = new Vector3(positionList[index].x, positionList[index].y);
	}

	public override void ToggleReached() {
		nodeImage.color = activeColor;
        reachedAnim.Play();
    }
}
