using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Custom loader to cache all the sprite data from images
// Didnt want to go through the whole sprite list everytime one is needed
// NOTE: this is a sprite list call-driven population,
//		so it will miss the food if there is no sprite
public class SpriteCacheManager : Singleton<SpriteCacheManager> {
	public Dictionary<string, Sprite> foodSpriteDictionary = new Dictionary<string, Sprite>();
	private static bool isCreated;

	void Awake(){
		// Make object persistent
		if(isCreated){
			// If There is a duplicate in the scene. delete the object and jump Awake
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		isCreated = true;
	}

	public Sprite GetSpriteData(string spriteName){
		if(foodSpriteDictionary.Count == 0){
			// Set up sprite list from data
			Sprite[] sprites = Resources.LoadAll<Sprite>("food");
			foreach(Sprite spr in sprites){
				foodSpriteDictionary.Add(spr.name, spr);	// Adding all sprites for now
			}
		}

		if(!foodSpriteDictionary.ContainsKey(spriteName)){
			Debug.LogError("Can not find key : " + spriteName);
		}

		return foodSpriteDictionary[spriteName];
	}
}
