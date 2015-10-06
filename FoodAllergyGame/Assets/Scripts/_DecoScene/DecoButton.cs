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

	public GameObject checkMark;

	public void Init(ImmutableDataDecoItem decoData){
		decoID = decoData.ID;
		gameObject.name = decoData.ID;
		decoNameText.text = LocalizationText.GetText(decoData.TitleKey);

		if(decoData.TitleKey != "None"){
			string spriteName = decoData.SpriteName;
			decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(spriteName);
		}
		else{
			decoImage.sprite = removeSprite;
		}

		RefreshButtonState();
	}

	// Change button colors based on the state of the decoration
	public void RefreshButtonState(){
		// Check if it was bought already
		if(DecoManager.IsDecoBought(decoID)){
			buttonImageTop.sprite = boughtSpriteTop;
			checkMark.SetActive(DecoManager.IsDecoActive(decoID) ? true : false);
		}
		else{
			checkMark.SetActive(false);
			buttonImageTop.sprite = unboughtSpriteTop;
		}
	}

	public void OnButtonClicked(){
		DecoManager.Instance.ShowCaseDeco(decoID);
	}
}
