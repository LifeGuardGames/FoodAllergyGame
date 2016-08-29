using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapNodeBase : MapNode {
	public GameObject rewardParent;
	public Text starNumberText;
	public List<Image> rewardList;
	public Animation reachedAnim;
	public Color activeColor;
	public Image nodeImage;

	private Dictionary<AssetTypes, List<string>> unlocksHash;
	private int currentRewardIndex = 0;
	private bool isStartTier;

	public void Init(ImmutableDataTiers tier, bool _isStartTier, CanvasScaler canvasScaler) {
		isStartTier = _isStartTier;
		if(_isStartTier) {
			transform.localPosition = new Vector3(0f, 0f, 0f);
			rewardParent.SetActive(false);
			starNumberText.enabled = false;
			nodeImage.color = activeColor;
        }
		else {
			transform.localPosition = new Vector3(0f, canvasScaler.referenceResolution.y, 0f);
			starNumberText.enabled = true;
			if(tier != null) {
				starNumberText.text = "Planet " + tier.TierNumber.ToString();

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
				starNumberText.text = "Coming Soon!";
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

	public override void ToggleReached(bool isSetup) {
		if(!isStartTier) {
			AudioManager.Instance.PlayClip("MapEndReach");
		}
		if(!isSetup) {
			AudioManager.Instance.PlayClip("MapNodeReach");
			reachedAnim.Play();
		}
		nodeImage.color = activeColor;
    }
}
