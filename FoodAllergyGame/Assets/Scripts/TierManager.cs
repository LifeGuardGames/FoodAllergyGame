using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TierManager : Singleton<TierManager> {

	private string tierXMLPrefix = "Tier";	// Prefix of the tier xml keys, ie. "Tier04"

	private int tier;						// Cached tier for use throughout game
	public int Tier{
		get{ return tier; }
	}

	private List<string> specialItemID;				// Special item IDs used for start notifications
	public List<string> SpecialItemID{
		get{ return specialItemID; }
	}

	void Awake() {
		specialItemID = new List<string>();
	}

	// Recalculate the tier given a certain algorithm
	// NOTE: Should be done on StartScene ONLY
	public void RecalculateTier(){
		tier = DataLoaderTiers.GetTierFromCash(DataManager.Instance.GameData.Cash.LastSeenTotalCash);
		int newTier = DataLoaderTiers.GetTierFromCash(DataManager.Instance.GameData.Cash.TotalCash);

		// NOTE: If there is a change in tier, run logic
		if(tier < newTier){
			SpecialTierUnlock(newTier);    // TODO support multiple tier increments
		}
		//this is here to prevent non tutorial special deco from being added to the game. It's a funnel for multiple unlocks
		if(specialItemID.Count > 0) {
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData(specialItemID[0]);
			DataManager.Instance.GameData.Decoration.BoughtDeco.Add(specialItemID[0], "");
			DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(decoData.Type);
			DataManager.Instance.GameData.Decoration.ActiveDeco.Add(decoData.Type, decoData.ID);
		}
		// Print out tier
		Debug.Log("Recalculated tier: " + tier + "     total cash: " + DataManager.Instance.GameData.Cash.TotalCash);
	}

	public void RemoveSpecialID() {
		specialItemID.RemoveAt(0);
	}

	public int GetMenuSlots(){
		return DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(tier)).MenuSlots;
	}

	// Loops through all previous events for a compiled list
	public List<string> GetEventsUnlocked(){
		List<string> eventsUnlocked = new List<string>();
		if(tier >= 0){
			for(int i = 0; i <= tier; i++){
				string[] eventsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(i)).EventsUnlocked;
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
				string[] foodsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(i)).FoodsUnlocked;
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
				string[] decosAtTier = DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(i)).DecorationsUnlocked;
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
				string[] assetsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(i)).StartArtAssets;
				foreach(string restaurantEvent in assetsAtTier){
					startArtAssets.Add(restaurantEvent);
				}
			}
		}
		return startArtAssets;
	}


	public string GetNewEvent(){
		if(DataManager.Instance.GameData.Decoration.DecoTutQueue.Count != 0){
			return DataManager.Instance.GameData.Decoration.DecoTutQueue[0];
		}
		else{
			List <string> eventList = GetEventsUnlocked();
			return eventList[UnityEngine.Random.Range(0, eventList.Count)];
		}
	}

	// Checks any special case for unlocking a tier
	// IMPORTANT NOTE: Make sure to set specialDecoID so notificationManager can pick it up!
	public void SpecialTierUnlock(int newTier){
		if(newTier >= 1 && tier < 1){
			specialItemID.Add("VIP00");
			DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTVIP");
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}

		if(newTier >= 5 && tier < 5) {
			specialItemID.Add("PlayArea00");
			DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTPlayArea");
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}
		tier = newTier;
		//TODO More special unlock logic here
	}
}
