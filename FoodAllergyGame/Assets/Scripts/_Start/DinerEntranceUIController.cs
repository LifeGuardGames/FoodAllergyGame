using UnityEngine;
using System.Collections.Generic;

public class DinerEntranceUIController : MonoBehaviour {

	public Animator dinerEntranceAnimator;

	public List<SpriteRenderer> starBaseList;
	public List<SpriteRenderer> starCoreList;
	public Sprite starShellSprite;
	public Sprite starBaseSprite;

	void Start() {
		// Populate star cores list from data
		for(int i = 0; i < starCoreList.Count; i++) {
			if(i < DataManager.Instance.GameData.Challenge.StarCoresEarned) {
				starCoreList[i].enabled = false;
			}
			else {
				starCoreList[i].enabled = true;
			}
		}

		// Populate star base list from tiers

		// TODO USE CUSTOM SPRITES (SETS OF 6)
		for(int i = 0; i < starBaseList.Count; i++) {
			Sprite loadedSprite = SpriteCacheManager.GetStarFillSpriteData(TierManager.Instance.CurrentTier, 6, i);
			if(loadedSprite != null) {
				starCoreList[i].sprite = loadedSprite;
			}
			else {
				starCoreList[i].enabled = false;
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
