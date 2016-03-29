using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// One item of the reward unlocks, a series of these are spawned and tracked by the RewardUIController
/// These handle their own animations for gift opening and tweening then theyre done
/// </summary>
public class RewardItem : MonoBehaviour {
	public Image itemSprite;
	public RotateAroundCenter sunRaysRotate;
	public Animator rewardItemAnimator;

	private AssetTypes assetType;
	private string itemID;

	public void InitItem(AssetTypes _assetType, string _itemID) {
		assetType = _assetType;
		itemID = _itemID;

		// Do self initialization here
		switch(assetType) {
			// ...
		}
	}

	public void AnimationAppear() {
		rewardItemAnimator.Play("Appear");
	}

	public void AnimationOpen() {
		rewardItemAnimator.Play("Open");
	}

	// On done button from UI, fly all the items into their respective locations
	public void FlyToEntrance() {
		Vector3 position;
        switch(assetType) {
			case AssetTypes.Challenge:
				position = StartManager.Instance.GetEntrancePosition(StartMenuEntrances.ChallengeEntrance);
				break;
			case AssetTypes.Customer:
			case AssetTypes.Food:
			case AssetTypes.Slot:
				position = StartManager.Instance.GetEntrancePosition(StartMenuEntrances.DinerEntrance);
				break;
			case AssetTypes.DecoBasic:
			case AssetTypes.DecoSpecial:
				position = StartManager.Instance.GetEntrancePosition(StartMenuEntrances.DecoEntrance);
				break;
		}

		// Do some conversion with position here
		// ...
	}
}
