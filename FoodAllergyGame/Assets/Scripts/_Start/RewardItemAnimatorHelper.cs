using UnityEngine;
using System.Collections;

// Called from RewardItem animator
public class RewardItemAnimatorHelper : MonoBehaviour {
	public RewardItem rewardItem;

	public void ShowDescription() {
		rewardItem.ShowDescription();
	}

	public void DestroySelf() {
		Destroy(transform.parent.gameObject, 0.5f);
	}
}
