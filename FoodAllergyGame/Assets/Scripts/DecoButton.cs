using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DecoButton : MonoBehaviour {

	public string decoID;
	public string deco;
	public SpriteRenderer decoSprite;
	public bool isBought;

	void Start(){
		this.GetComponent<Button>().onClick.AddListener(this.ChangeSet);

	}

	public void Init(ImmutableDataDecoItem _deco){
		deco = _deco.type;
		decoID = _deco.id;
		Debug.Log (_deco.id);
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID)){
			isBought = true;
		}
	}

	public void ChangeSet(){
		DecoManager.Instance.ChangeSet(deco, decoID);
		if(DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey(decoID)){
			isBought = true;
			//TODO look bought
		}
		if(isBought){
			decoSprite.sprite = SpriteCacheManager.Instance.GetDecoSpriteData(decoID);
			//TODO look selected
		}
	}

}
