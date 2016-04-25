using UnityEngine;
using UnityEngine.UI;

public class DecoButton : MonoBehaviour {
	private string decoID;

	public Image buttonImageTop;

	public Image decoImage;
	public GameObject newTag;

	public Sprite removeSprite;
	public Sprite unboughtSpriteTop;
	public Sprite boughtSpriteTop;
	public Sprite lockedSpriteTop;

	public GameObject checkMark;
	public GameObject padlock;

	private bool isUnlocked = false;			// Cached on instantiate

	public void Init(ImmutableDataDecoItem decoData){
		decoID = decoData.ID;
		gameObject.name = decoData.ID;
		
		string spriteName = decoData.SpriteName;
		decoImage.sprite = SpriteCacheManager.GetDecoSpriteData(spriteName);

		isUnlocked = DecoManager.Instance.IsDecoUnlocked(decoID);
		if(DataManager.Instance.GameData.Decoration.NewDeco.Contains(decoData.ID) && isUnlocked) {
			DataManager.Instance.GameData.Decoration.NewDeco.Remove(decoData.ID);
			newTag.SetActive(true);
        }
		else {
			newTag.SetActive(false);
		}
		RefreshButtonState();
	}

	// Change button colors based on the state of the decoration
	public void RefreshButtonState(){
		if(isUnlocked) {
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
			decoImage.color = new Color(0.5f, 0.5f, 0.5f);
            buttonImageTop.sprite = lockedSpriteTop;
        }
	}

	public void OnButtonClicked(){
		DecoManager.Instance.ShowCaseDeco(decoID);
	}
}
