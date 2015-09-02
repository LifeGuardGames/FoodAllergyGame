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
			ImmutableDataDecoItem activeDeco = DataManager.Instance.GetActiveDecoData(DecoTypes.Table);
			sprite.sprite = SpriteCacheManager.GetDecoSpriteData(activeDeco.SpriteName);
		}
		else{
			DecoManager.Instance.sceneObjects.Add(type, this.gameObject);
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(type);
			sprite.sprite = sprite.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
			if(kitchenBack != null){
				kitchenBack.sprite = SpriteCacheManager.GetDecoSpriteData(decoData.SpriteName);
			}
		}
	}
}
