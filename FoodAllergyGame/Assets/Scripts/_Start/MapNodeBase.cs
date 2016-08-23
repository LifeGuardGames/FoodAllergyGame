﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapNodeBase : MonoBehaviour {
	public GameObject rewardParent;
	public Text starNumberText;
	public List<Image> rewardList;

	private Dictionary<AssetTypes, List<string>> unlocksHash;
	private int currentRewardIndex = 0;

	public void Init(ImmutableDataTiers tier, bool isStartTier, CanvasScaler canvasScaler) {
		if(isStartTier) {
			transform.localPosition = new Vector3(0f, 0f, 0f);
			rewardParent.SetActive(false);
			starNumberText.enabled = false;
        }
		else {
			transform.localPosition = new Vector3(0f, canvasScaler.referenceResolution.y, 0f);
			starNumberText.enabled = true;
			starNumberText.text = "Planet " + tier.TierNumber.ToString();
			if(tier != null) {
				// Fill the rewards in a certain order by type
				unlocksHash = TierManager.Instance.GetAllUnlocksAtTier(tier.TierNumber);
				PopulateRewardListType(AssetTypes.Customer);
				PopulateRewardListType(AssetTypes.DecoSpecial);
				PopulateRewardListType(AssetTypes.DecoBasic);
				PopulateRewardListType(AssetTypes.Food);
				PopulateRewardListType(AssetTypes.Challenge);
				PopulateRewardListType(AssetTypes.Slot);
				PopulateRewardListBlank();
			}
			else {
				rewardParent.SetActive(false);
			}
		}
	}

	private void PopulateRewardListType(AssetTypes assetType) {
		foreach(string customerID in unlocksHash[assetType]) {
			if(currentRewardIndex < rewardList.Count) {
				rewardList[currentRewardIndex].sprite = SpriteCacheManager.SpriteGetRewardSpriteByType(assetType, customerID);
				currentRewardIndex++;
			}
		}
	}

	// Pad the rest of the unused sprites here
	private void PopulateRewardListBlank() {
		for(int i = currentRewardIndex; i < rewardList.Count; i++) {
			rewardList[i].enabled = false;
		}
	}
}
