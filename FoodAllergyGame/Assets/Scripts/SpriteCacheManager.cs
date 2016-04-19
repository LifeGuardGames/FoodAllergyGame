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

	public static Sprite GetLoadingImageData(string spriteName) {
		Sprite sprite = Resources.Load<Sprite>(spriteName);
		return sprite;
	}

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

	public static Sprite GetSlotSpriteData(int slots){
		Sprite sprite = Resources.Load<Sprite>("SlotCoin" + slots.ToString());
		return sprite;
	}

	public static Sprite GetSlotItemSpriteData() {
		Sprite sprite = Resources.Load<Sprite>("ItemSlot");
		return sprite;
	}

	public static Sprite GetEpiPenTokenSpriteData(int tokenNumber) {
		Sprite sprite = Resources.Load<Sprite>("EpiPenToken" + tokenNumber.ToString());
		return sprite;
	}

	public static Sprite GetChallengeButton(ChallengeReward rewardType) {
		Sprite sprite = Resources.Load<Sprite>("ChallengeButton" + rewardType.ToString());
		return sprite;
	}

	public static Sprite GetChallengeItemSpriteData() {
		Sprite sprite = Resources.Load<Sprite>("ItemChallenge");
		return sprite;
	}

	public static Sprite GetTrophySpriteData(ChallengeReward rewardType) {
		Sprite sprite = Resources.Load<Sprite>("Trophy" + rewardType.ToString());
		return sprite;
	}

	/// <summary>
	/// Used for initializing
	/// </summary>
	/// <param name="tiers">Your current tier</param>
	/// <param name="totalTiersInOneStar">Number of tiers that complete one star</param>
	/// <param name="starIndex">Star index in question, initializing stars in a list</param>
	/// <returns>Sprite of star fill of the current star index, null if empty</returns>
	public static Sprite GetStarFillSpriteData(int tiers, int totalTiersInOneStar, int starIndex) {
        int suffixFill = 0;
		int starAux = (starIndex + 1) * totalTiersInOneStar;
		
		if(starAux > tiers) {	// Partial or empty
			if(starAux - tiers < totalTiersInOneStar) {
				suffixFill = tiers % totalTiersInOneStar;
            }
			else {
				return null;
			}
		}
		else {					// Full
			suffixFill = totalTiersInOneStar;
        }
		return GetStarFillHelper(suffixFill, totalTiersInOneStar);
	}

	public static Sprite GetStarFillHelper(int suffixFill, int totalTiersInOneStar) {
		if(suffixFill == 0) {
			return null;
		}
        string loadString = suffixFill == totalTiersInOneStar ? "StarsBase" : "StarPiece" + suffixFill;
		Debug.Log("Loading " + loadString);
		Sprite sprite = Resources.Load<Sprite>(loadString);
		return sprite;
	}
}
