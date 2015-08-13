using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoButton : MonoBehaviour {

	public string decoID;
	public DecoTypes deco;
	public SpriteRenderer decoSprite;
	public bool isBought;

	void Start(){
		this.GetComponent<Button>().onClick.AddListener(this.ChangeSet);
	}

	public void Init(DecoTypes _deco, string _id){
		deco = _deco;
		decoID = _id;
	}

	public void ChangeSet(){
		if(isBought){
			decoSprite.sprite = SpriteCacheManager.Instance.GetDecoSpriteData(decoID);
			DataManager.Instance.GameData.Decoration.currDiner.Remove(deco);
			DataManager.Instance.GameData.Decoration.currDiner.Add(deco,decoID);
		}
		else{
			BuyItem();
		}
	}
	public void BuyItem(){
		DataManager.Instance.GameData.Decoration.BoughtDeco.Add(decoID, " ");
		isBought = true;
		ChangeSet();
	}
}
