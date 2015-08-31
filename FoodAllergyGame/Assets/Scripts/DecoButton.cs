using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecoButton : MonoBehaviour {
	public Image decoImage;
	public Text priceText;
	public Text decoNameText;
	private string decoID;
	private bool isBought = false;

	void Start(){
		this.GetComponent<Button>().onClick.AddListener(this.ChangeSet);
	}

	public void Init(ImmutableDataDecoItem decoData){
		decoID = decoData.ID;
		gameObject.name = decoData.ID;
		decoNameText.text = decoData.ButtonTitle;
		decoImage.sprite = SpriteCacheManager.Instance.GetDecoSpriteData(decoData.SpriteName);

		// Check if it was bought already or show the price
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID)){
			isBought = true;
		}
		else{
			priceText.text = decoData.Cost.ToString();
		}
	}

	public void ChangeSet(){
		DecoManager.Instance.ChangeSet(decoID);
	}
}
