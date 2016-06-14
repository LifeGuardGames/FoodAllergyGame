using UnityEngine;
using UnityEngine.UI;

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
	private string titleKey = null;
	private string descriptionKey = null;
	private RewardUIController rewardUIController;
	private ImmutableDataFood foodDataAux;

	private bool isOpened;
	public bool IsOpened {
		get { return isOpened; }
		set { isOpened = value; }
	}

	public void InitItem(AssetTypes _assetType, string _itemID, RewardUIController _rewardUIController) {
		assetType = _assetType;
		itemID = _itemID;
		rewardUIController = _rewardUIController;

		// Do self initialization here
		// Note: Some assets should have capsule while foods should not
		switch(assetType) {
			case AssetTypes.Challenge:
				ImmutableDataChallenge challengeData = DataLoaderChallenge.GetData(itemID);
                titleKey = "ChallengeItemTitle";
                descriptionKey = challengeData.Title;
				itemSprite.sprite = SpriteCacheManager.GetChallengeItemSpriteData();

				isOpened = true;
				break;
			case AssetTypes.Customer:
				ImmutableDataCustomer customerData = DataLoaderCustomer.GetData(itemID);
				titleKey = customerData.CustomerNameKey;
				descriptionKey = customerData.CustomerDescription;
				itemSprite.sprite = SpriteCacheManager.GetCustomerSpriteData(customerData.SpriteName);

				isOpened = false;
				break;
			case AssetTypes.DecoBasic:
			case AssetTypes.DecoSpecial:
				ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(itemID);
				titleKey = decoData.TitleKey;
				if(!string.Equals(decoData.DescriptionKey, "None")){
					descriptionKey = decoData.DescriptionKey;
				}
				itemSprite.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);

				isOpened = false;
				break;
			case AssetTypes.Food:
				ImmutableDataFood foodData = DataLoaderFood.GetData(itemID);
				foodDataAux = foodData;
				titleKey = foodData.FoodNameKey;
				itemSprite.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);

				isOpened = true;
				break;
			case AssetTypes.Slot:
				titleKey = "NewFoodSlotItem";
				itemSprite.sprite = SpriteCacheManager.GetSlotItemSpriteData();

				isOpened = true;
				break;
		}

		// Hide capsule in the beginning
		rewardItemAnimator.Play("CapsuleHide");
	}

	public void AnimationAppear() {
		AudioManager.Instance.PlayClip("CapsuleAppearSound");
		
		// Check if already open when appearing
		if(isOpened) {
			rewardItemAnimator.Play("CapsuleDirectlyShowFood");
		}
		else {
			rewardItemAnimator.Play("CapsuleAppear");
		}
	}

	public void OnCapsuleClicked() {
		if(!IsOpened && rewardUIController.IsCapsuleClickable(this)) {
			AudioManager.Instance.PlayClip("CapsuleOpenSound");
			rewardItemAnimator.Play("CapsuleOpen");
			IsOpened = true;
			rewardUIController.DoneButtonToggleCheck();
        }
	}

	// Called from animation vent and on item change
	public void ShowDescription() {
		if(!IsOpened) {
			rewardUIController.ShowInfo(null, null);
		}
		else {
			if(assetType == AssetTypes.Food) {
				Allergies allergyAux1 = foodDataAux.AllergyList.Count >= 1 ? foodDataAux.AllergyList[0] : Allergies.None;
				Allergies allergyAux2 = foodDataAux.AllergyList.Count >= 2 ? foodDataAux.AllergyList[1] : Allergies.None;
				Allergies allergyAux3 = foodDataAux.AllergyList.Count >= 3 ? foodDataAux.AllergyList[2] : Allergies.None;
				rewardUIController.ShowInfo(titleKey, foodDataAux.AllergyList.Count, allergyAux1, allergyAux2, allergyAux3);
			}
			else {
				rewardUIController.ShowInfo(titleKey, descriptionKey);
			}
		}
    }

	// On done button from UI, fly all the items into their respective locations
/*	public void FlyToEntrance() {
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
	*/
}
