using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecoAdder : MonoBehaviour {
	public DecoTypes type;
	public SpriteRenderer kitchenBack;
	public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		if(type == DecoTypes.Table){
			sprite.sprite = SpriteCacheManager.Instance.GetDecoSpriteData(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.currDiner[DecoTypes.Table]).SpriteName);
		}
		else{
			DecoManager.Instance.sceneObjects.Add(type, this.gameObject);
			ImmutableDataDecoItem _sprite = DataLoaderDecoItem.GetData(DecoManager.Instance.SetUp(type));
			sprite.sprite = SpriteCacheManager.Instance.GetDecoSpriteData(_sprite.SpriteName);
			if(kitchenBack != null){
				kitchenBack.sprite = SpriteCacheManager.Instance.GetDecoSpriteData(_sprite.SecondarySprite);
			}
		}
	}
}
