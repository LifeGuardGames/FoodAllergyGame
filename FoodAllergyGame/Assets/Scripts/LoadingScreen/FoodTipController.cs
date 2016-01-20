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
	public TweenToggleDemux tipDemux;
	public Image foodImage;
	public Image allergy1Image;
	public Image allergy2Image;
	public Image allergy3Image;

	private int unlockedFoodCount = -1;		// Init to force initialization
	private List<ImmutableDataFood> internalFoodList;

	void Start() {
		RefreshInternalList();
	}

	private void RefreshInternalList() {
		List<ImmutableDataFood> newFoodList = DataLoaderFood.GetDataListWithinTier(TierManager.Instance.Tier);
		if(newFoodList.Count != unlockedFoodCount){
			internalFoodList = newFoodList;
			unlockedFoodCount = internalFoodList.Count;

			// TODO Reset other stuff here
		}
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
		foreach(Allergies allergy in randomFoodData.AllergyList) {
			int currentIndex = 0;
			if(allergy == Allergies.None) {
				break;
			}
			else if(currentIndex == 0){
				allergy1Image.gameObject.SetActive(true);
				allergy1Image.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
			}
			else if(currentIndex == 1) {
				allergy2Image.gameObject.SetActive(true);
				allergy2Image.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
			}
			else {
				allergy3Image.gameObject.SetActive(true);
				allergy3Image.sprite = SpriteCacheManager.GetAllergySpriteData(allergy);
			}
			currentIndex++;
		}

        tipDemux.Show();
    }

	public void HideFoodTip() {
		tipDemux.Hide();
	}
}
