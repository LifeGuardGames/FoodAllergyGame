using UnityEngine;
using System.Collections.Generic;

public class DinerEntranceUIController : MonoBehaviour {

	public Animator dinerEntranceAnimator;
	public List<SpriteRenderer> starBaseList;
	public List<SpriteRenderer> starCoreList;

	void Start() {
		// Populate star cores list from data
		for(int i = 0; i < starCoreList.Count; i++) {
			if(i < DataManager.Instance.GameData.Challenge.StarCoresEarned || !DataManager.Instance.GameData.Tutorial.IsComicViewed) {
				starCoreList[i].enabled = true;
			}
			else {
				starCoreList[i].enabled = false;
			}
		}

		// Populate star base list from tiers
		for(int i = 0; i < starBaseList.Count; i++) {
			Sprite loadedSprite;
            if(!DataManager.Instance.GameData.Tutorial.IsComicViewed) {
				 loadedSprite = SpriteCacheManager.GetStarFillSpriteData(36, 6, i);
            }
			else {
				 loadedSprite = SpriteCacheManager.GetStarFillSpriteData(TierManager.Instance.CurrentTier, 6, i);
			}
			if(loadedSprite != null) {
				starBaseList[i].sprite = loadedSprite;
			}
			else {
				starBaseList[i].enabled = false;
            }
        }
	}

	void OnMouseUpAsButton(){
		StartManager.Instance.OnPlayButtonClicked();
	}

	public void ToggleClickable(bool isClickable){
		GetComponent<BoxCollider2D>().enabled = isClickable;
		dinerEntranceAnimator.SetBool("IsClickable", isClickable);
    }
}
