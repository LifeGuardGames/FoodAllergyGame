using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controller to show food tips
/// The food showing is forced to cycle randomly until all is shown (choose randomly form bucket and remove picked each time)
/// Once all the elements have shown, reset the bucket and start over again
/// If new foods are unlocked, reset your list
/// </summary>
public class FoodTipController : MonoBehaviour {
	public Animator tipAnimator;	// Animates the nodes
	public Image allergy1Image;
	public Image allergy2Image;
	public Image allergy3Image;
	public Image foodImage;
	public Text foodText;

	private List<ImmutableDataFood> internalFoodList;

	void Start() {
		RefreshInternalList();
		tipAnimator.SetBool("IsShow", false);
	}

	private void RefreshInternalList() {
		List<ImmutableDataFood> newFoodList = DataLoaderFood.GetDataListWithinTier(TierManager.Instance.CurrentTier);
		internalFoodList = newFoodList;
	}

	private ImmutableDataFood GetRandomFood() {
		if(internalFoodList.Count == 0) {
			RefreshInternalList();
		}
		int randomIndex = UnityEngine.Random.Range(0, internalFoodList.Count);
		ImmutableDataFood randomFoodData = internalFoodList[randomIndex];
		internalFoodList.RemoveAt(randomIndex);
		return randomFoodData;
	}

	public void ShowFoodTip() {
		ImmutableDataFood randomFoodData = GetRandomFood();
		foodImage.sprite = SpriteCacheManager.GetFoodSpriteData(randomFoodData.SpriteName);
		foodText.text = LocalizationText.GetText(randomFoodData.FoodNameKey);
		
		foreach(Transform child in allergy2Image.transform) {
			child.gameObject.SetActive(true);
		}
		foreach(Transform child in allergy3Image.transform) {
			child.gameObject.SetActive(true);
		}

		// Populate the allergies and turn off unused ones
		switch(randomFoodData.AllergyList.Count) {
			case 1:
				allergy1Image.sprite = SpriteCacheManager.GetAllergySpriteData(randomFoodData.AllergyList[0]);
				allergy2Image.enabled = false;
				foreach(Transform child in allergy2Image.transform) {
					child.gameObject.SetActive(false);
				}
				allergy3Image.enabled = false;
				foreach(Transform child in allergy3Image.transform) {
					child.gameObject.SetActive(false);
				}
				break;
			case 2:
				allergy1Image.sprite = SpriteCacheManager.GetAllergySpriteData(randomFoodData.AllergyList[0]);
				allergy2Image.sprite = SpriteCacheManager.GetAllergySpriteData(randomFoodData.AllergyList[1]);
				allergy3Image.enabled = false;
				foreach(Transform child in allergy3Image.transform) {
					child.gameObject.SetActive(false);
				}
				break;
			case 3:
				allergy1Image.sprite = SpriteCacheManager.GetAllergySpriteData(randomFoodData.AllergyList[0]);
				allergy2Image.sprite = SpriteCacheManager.GetAllergySpriteData(randomFoodData.AllergyList[1]);
				allergy3Image.sprite = SpriteCacheManager.GetAllergySpriteData(randomFoodData.AllergyList[2]);
				break;
			default:
				Debug.LogError("Invalid allergy list count " + randomFoodData.AllergyList.Count);
				break;
		}

		tipAnimator.SetBool("IsShow", true);
	}

	public void HideFoodTip() {
		tipAnimator.SetBool("IsShow", false);
	}
}
