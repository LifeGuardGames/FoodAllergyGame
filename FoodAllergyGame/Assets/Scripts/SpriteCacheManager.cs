using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Custom loader to cache all the sprite data from images
// Didnt want to go through the whole sprite list everytime one is needed
// NOTE: this is a sprite list call-driven population,
//		so it will miss the food if there is no sprite
public class SpriteCacheManager : Singleton<SpriteCacheManager> {

//	public Dictionary<string, Sprite> foodSpriteDictionary = new Dictionary<string, Sprite>();
//	public Sprite GetFoodSpriteData(string spriteName){
//		if(foodSpriteDictionary.Count == 0){
//			// Set up sprite list from data
//			Sprite[] sprites = Resources.LoadAll<Sprite>("food");
//			foreach(Sprite spr in sprites){
//				foodSpriteDictionary.Add(spr.name, spr);	// Adding all sprites for now
//			}
//		}
//
//		if(!foodSpriteDictionary.ContainsKey(spriteName)){
//			Debug.LogError("Can not find key : " + spriteName);
//		}
//
//		return foodSpriteDictionary[spriteName];
//	}

	public static Sprite GetFoodSpriteData(string spriteName){
		Sprite sprite = Resources.Load<Sprite>(spriteName);
		return sprite;
	}

	public static Sprite GetCustomerSpriteData(string spriteName){
		Sprite sprite = Resources.Load<Sprite>(spriteName);
		return sprite;
	}

	public static Sprite GetDecoSpriteDataByID(string decoID){
		ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(decoID);
		return GetDecoSpriteData(decoData.SpriteName);
	}

	public static Sprite GetDecoSpriteData(string spriteName){
		Sprite sprite = Resources.Load<Sprite>(spriteName);
		return sprite;
	}

	public static Sprite GetAllergySpriteData(Allergies allergyEnum){
		Sprite sprite = Resources.Load<Sprite>("Allergy" + allergyEnum.ToString());
		return sprite;
	}
}
