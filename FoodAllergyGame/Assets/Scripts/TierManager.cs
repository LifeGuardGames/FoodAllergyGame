﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TierManager : MonoBehaviour {

	private string tierXMLPrefix = "Tier";	// Prefix of the tier xml keys, ie. "Tier04"
	private int tier;						// Cached tier for use throughout game

	// Recalculate the tier given a certain algorithm, should be done on StartScene only
	public void RecalculateTier(){
		int progress = DataManager.Instance.GameData.Cash.TotalCash;
		tier = progress / 200;

		// Print out tier
		Debug.Log("Recalculated tier: " + tier + "     total cash: " + progress);
	}

	public int GetMenuSlots(){
		return DataLoaderTiers.GetData(tierXMLPrefix + tier.ToString()).MenuSlots;
	}

	// Loops through all previous events for a compiled list
	public List<string> GetEventsUnlocked(){
		List<string> eventsUnlocked = null;
		if(tier >= 0){
			for(int i = 0; i <= tier; i++){
				string[] eventsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + i.ToString()).EventsUnlocked;
				foreach(string restaurantEvent in eventsAtTier){
					eventsUnlocked.Add(restaurantEvent);
				}
			}
		}
		return eventsUnlocked;
	}

	public List<string> GetFoodsUnlocked(){
		List<string> foodsUnlocked = null;
		if(tier >= 0){
			for(int i = 0; i <= tier; i++){
				string[] foodsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + i.ToString()).FoodsUnlocked;
				foreach(string restaurantEvent in foodsAtTier){
					foodsUnlocked.Add(restaurantEvent);
				}
			}
		}
		return foodsUnlocked;
	}

	public List<string> GetDecorationsUnlocked(){
		List<string> deocrationsUnlocked = null;
		if(tier >= 0){
			for(int i = 0; i <= tier; i++){
				string[] decosAtTier = DataLoaderTiers.GetData(tierXMLPrefix + i.ToString()).DecorationsUnlocked;
				foreach(string restaurantEvent in decosAtTier){
					deocrationsUnlocked.Add(restaurantEvent);
				}
			}
		}
		return deocrationsUnlocked;
	}

	public List<string> GetStartArtAssets(){
		List<string> startArtAssets = null;
		if(tier >= 0){
			for(int i = 0; i <= tier; i++){
				string[] assetsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + i.ToString()).StartArtAssets;
				foreach(string restaurantEvent in assetsAtTier){
					startArtAssets.Add(restaurantEvent);
				}
			}
		}
		return startArtAssets;
	}
}
