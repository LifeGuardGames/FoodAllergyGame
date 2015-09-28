using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecoButton : MonoBehaviour {
	private string decoID;
	private int costAux;

	public Image buttonImage;
	public Image decoImage;
	public Text decoNameText;

	public GameObject priceParent;
	public Text priceText;

	public Sprite removeSprite;
	public Sprite unboughtSprite;
	public Sprite unequippedSprite;
	public Sprite equippedSprite;

	public GameObject checkMark;

	public void Init(ImmutableDataDecoItem decoData){
		decoID = decoData.ID;
		gameObject.name = decoData.ID;
		decoNameText.text = LocalizationText.GetText(decoData.ButtonTitleKey);
		costAux = decoData.Cost;

		if(decoData.ButtonTitleKey != "None"){
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
		// Check if it was bought already or show the price
		if(DecoManager.IsDecoBought(decoID)){
			priceParent.gameObject.SetActive(false);
			if(DecoManager.IsDecoActive(decoID)){
				buttonImage.sprite = equippedSprite;
				checkMark.SetActive(true);
			}
			else{
				buttonImage.sprite = unequippedSprite;
				checkMark.SetActive(false);
			}
		}
		else{
			checkMark.SetActive(false);
			priceParent.gameObject.SetActive(true);
			priceText.text = costAux.ToString();
			buttonImage.sprite = unboughtSprite;
		}
	}

	public void OnButtonClicked(){
		DecoManager.Instance.SetDeco(decoID);
		BroadcastToDecoButtons();
	}

	// Tell all other decoButtons to refresh their states
	public void BroadcastToDecoButtons(){
		transform.parent.gameObject.BroadcastMessage("RefreshButtonState", SendMessageOptions.DontRequireReceiver);
	}
}
