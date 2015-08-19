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
			sprite.sprite = Resources.Load <Sprite>(DataLoaderDecoItem.GetData(DataManager.Instance.GameData.Decoration.currDiner[DecoTypes.Table]).SpriteName);
		}
		else{
			DecoManager.Instance.SceneObjects.Add(type, this.gameObject);
			ImmutableDataDecoItem _sprite = DataLoaderDecoItem.GetData(DecoManager.Instance.setUp(type));
			sprite.sprite = Resources.Load<Sprite>(_sprite.SpriteName);
			if(kitchenBack != null){
				kitchenBack.sprite = Resources.Load<Sprite>(_sprite.SecondarySprite);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
