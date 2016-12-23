using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class RewardInfoController : MonoBehaviour {
	public Image itemSprite;
	public Text textTitle;
	public Text textDescription;
	public Image allergyImage1;
	public Image allergyImage2;
	public Image allergyImage3;
	public TweenToggleDemux rewardInfoTween;
	
	private RewardUIController rewardUIController;

	public void ShowInfo(AssetTypes assetType, string itemID) {
        string titleKey = "";
        string descriptionKey = "";
		allergyImage1.enabled = false;
		allergyImage2.enabled = false;
		allergyImage3.enabled = false;
		switch(assetType) {
			case AssetTypes.Challenge:
				ImmutableDataChallenge challengeData = DataLoaderChallenge.GetData(itemID);
				titleKey = "ChallengeItemTitle";
				descriptionKey = challengeData.Title;
				itemSprite.sprite = SpriteCacheManager.GetChallengeItemSpriteData();
				break;
			case AssetTypes.Customer:
				ImmutableDataCustomer customerData = DataLoaderCustomer.GetData(itemID);
				titleKey = customerData.CustomerNameKey;
				descriptionKey = customerData.CustomerDescription;
				itemSprite.sprite = SpriteCacheManager.GetCustomerSpriteData(customerData.SpriteName);
				break;
			case AssetTypes.DecoBasic:
			case AssetTypes.DecoSpecial:
				ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(itemID);
				titleKey = decoData.TitleKey;
				if(!string.Equals(decoData.DescriptionKey, "None")) {
					descriptionKey = decoData.DescriptionKey;
				}
				itemSprite.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
				break;
			case AssetTypes.Food:
				ImmutableDataFood foodData = DataLoaderFood.GetData(itemID);
				titleKey = foodData.FoodNameKey;
				itemSprite.sprite = SpriteCacheManager.GetFoodSpriteData(foodData.SpriteName);
				int numberOfAllergies = foodData.AllergyList.Count;

				if(numberOfAllergies == 1) {
					allergyImage1.enabled = true;
					allergyImage1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
					allergyImage2.enabled = false;
					allergyImage3.enabled = false;
				}
				else if(numberOfAllergies == 2) {
					allergyImage1.enabled = true;
					allergyImage1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
					allergyImage2.enabled = true;
					allergyImage2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
					allergyImage3.enabled = false;
				}
				else if(numberOfAllergies == 3) {
					allergyImage1.enabled = true;
					allergyImage1.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[0]);
					allergyImage2.enabled = true;
					allergyImage2.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[1]);
					allergyImage3.enabled = true;
					allergyImage3.sprite = SpriteCacheManager.GetAllergySpriteData(foodData.AllergyList[2]);
				}
				break;
			case AssetTypes.Slot:
				titleKey = "NewFoodSlotItem";
				itemSprite.sprite = SpriteCacheManager.GetSlotItemSpriteData();
				break;
			case AssetTypes.StarPiece:
				titleKey = "StarPiece";
				itemSprite.sprite = SpriteCacheManager.GetStarPieceItemSpriteData();
				break;
		}

		AudioManager.Instance.PlayClip(assetType == AssetTypes.StarPiece ? "RewardStarPiece" : "RewardItem");

		textTitle.text = !string.IsNullOrEmpty(titleKey) ? LocalizationText.GetText(titleKey) : "";
		textDescription.text = "";

		rewardInfoTween.Show();
	}

	public void HideInfo() {
		rewardInfoTween.Hide();
	}
}
