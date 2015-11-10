using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecoButton : MonoBehaviour {
	private string decoID;

	public Image buttonImageTop;

	public Image decoImage;
	public Text decoNameText;

	public Sprite removeSprite;
	public Sprite unboughtSpriteTop;
	public Sprite boughtSpriteTop;
	public Sprite lockedSpriteTop;

	public GameObject checkMark;
	public GameObject padlock;

	private bool isLocked = false;			// Cached on instantiate

	public void Init(ImmutableDataDecoItem decoData){
		decoID = decoData.ID;
		gameObject.name = decoData.ID;
		decoNameText.text = LocalizationText.GetText(decoData.TitleKey);
		
		string spriteName = decoData.SpriteName;
		decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(spriteName);

		isLocked = DecoManager.Instance.IsDecoUnlocked(decoID);

		RefreshButtonState();
	}

	// Change button colors based on the state of the decoration
	public void RefreshButtonState(){
		if(!isLocked) {
			padlock.SetActive(false);

			// Check if it was bought already
			if(DecoManager.IsDecoBought(decoID)){
				checkMark.SetActive(DecoManager.IsDecoActive(decoID) ? true : false);
				buttonImageTop.sprite = boughtSpriteTop;
			}
			else{
				checkMark.SetActive(false);
				buttonImageTop.sprite = unboughtSpriteTop;
			}
		}
		else {
			checkMark.SetActive(false);
			padlock.SetActive(true);
            buttonImageTop.sprite = lockedSpriteTop;
        }
	}

	public void OnButtonClicked(){
		DecoManager.Instance.ShowCaseDeco(decoID);
	}
}
