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
			// TODO Referencing data directly... change?
			ImmutableDataDecoItem activeDeco = DataManager.Instance.GetActiveDecoData(DecoTypes.Table);
			sprite.sprite = Resources.Load <Sprite>(activeDeco.SpriteName);
		}
		else{
			DecoManager.Instance.sceneObjects.Add(type, this.gameObject);
			ImmutableDataDecoItem decoData = DataManager.Instance.GetActiveDecoData(type);
			sprite.sprite = Resources.Load<Sprite>(decoData.SpriteName);
			if(kitchenBack != null){
				kitchenBack.sprite = Resources.Load<Sprite>(decoData.SecondarySprite);
			}
		}
	}
}
