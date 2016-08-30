using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapNodeMid : MapNode{
	public Animation reachedAnim;
	public Image nodeImage;
	public GameObject rewardParent;
	public GameObject capsuleParent1;
	public GameObject capsuleParent2;
	public GameObject capsuleParent3;

	public void Init(ImmutableDataTiers tier, int index, List<Vector2> positionList, int nodeRewardCount) {
		transform.localPosition = new Vector3(positionList[index].x, positionList[index].y);

		// Used the random number as modulo to get random planet
		int randomX = Mathf.Abs((int)positionList[index].x);
        nodeImage.sprite = SpriteCacheManager.GetMapStarSpriteByIndex(randomX % 10);
        float size = UnityEngine.Random.Range(60f, 100f);
		nodeImage.rectTransform.sizeDelta = new Vector2(size, size);

		if(nodeRewardCount >= 1) {
			capsuleParent1.SetActive(true);
        }
		if(nodeRewardCount >= 2) {
			capsuleParent2.SetActive(true);
		}
		if(nodeRewardCount >= 3) {
			capsuleParent3.SetActive(true);
		}
	}

	public override void ToggleReached(bool isSetup) {
		if(!isSetup) {
			AudioManager.Instance.PlayClip("MapNodeReach");
			reachedAnim.Play();
			capsuleParent1.SetActive(false);
			capsuleParent2.SetActive(false);
			capsuleParent3.SetActive(false);
		}
		else {	// Turn off rewards
			rewardParent.SetActive(false);
        }
    }
}
