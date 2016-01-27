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

	private List<string> specialItemID;		// Special item IDs used for start notifications
	public List<string> SpecialItemID{
		get{ return specialItemID; }
	}

	private Dictionary<AssetTypes, List<string>> currentTierUnlocks;	// A hash of all the types of unlocks with their tiers
	public Dictionary<AssetTypes, List<string>> CurrentTierUnlocks{
		get{ return currentTierUnlocks; }
	}

	void Awake() {
		specialItemID = new List<string>();
	}

	// Recalculate the tier given a certain algorithm
	// NOTE: Should be done on StartScene ONLY
	public void RecalculateTier(){
		tier = DataLoaderTiers.GetTierFromCash(CashManager.Instance.LastSeenTotalCash);
		int newTier = DataLoaderTiers.GetTierFromCash(CashManager.Instance.TotalCash);

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
		Debug.Log("Recalculated tier: " + tier + "     total cash: " + CashManager.Instance.TotalCash);
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
			int rand = UnityEngine.Random.Range(0, eventList.Count);
            Debug.Log(rand);
			return eventList[rand];
		}
	}

	// Checks any special case for unlocking a tier
	// IMPORTANT NOTE: Make sure to set specialDecoID so notificationManager can pick it up!
	public void SpecialTierUnlock(int newTier){
		if(newTier >= 2 && tier < 2){
			specialItemID.Add("VIP00");
			DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTVIP");
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}

		if(newTier >= 4 && tier < 4) {
			specialItemID.Add("PlayArea00");
			DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTPlayArea");
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}
		if(newTier >= 6 && tier < 6) {
			specialItemID.Add("FlyThru00");
			DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTFlyThru");
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}
		tier = newTier;
		//TODO More special unlock logic here
	}

	/// <summary>
	/// Given a tier, get all the IDs of all the assets that are unlocked
	/// includes: Customers, BasicDeco, Special Deco, FoodItems, Slots, Challenges, Challenges
	/// NOTE: All AssetTypes will always be populated, check empty value list for empty unlock value instead
	/// </summary>
	/// <param name="tier">Tier</param>
	public Dictionary<AssetTypes, List<string>> GetAllUnlocksAtTier(int tier){
		Dictionary<AssetTypes, List<string>> unlockHash = new Dictionary<AssetTypes, List<string>>();
		unlockHash.Add(AssetTypes.Customer, DataLoaderCustomer.GetIDUnlockedAtTier(tier));
		unlockHash.Add(AssetTypes.DecoSpecial, DataLoaderDecoItem.GetBasicDecoIDUnlockedAtTier(tier));
		unlockHash.Add(AssetTypes.DecoBasic,  DataLoaderDecoItem.GetSpecialDecoIDUnlockedAtTier(tier));
		unlockHash.Add(AssetTypes.Food, DataLoaderFood.GetIDUnlockedAtTier(tier));
		unlockHash.Add(AssetTypes.Challenge, DataLoaderChallenge.GetIDUnlockedAtTier(tier));

		List<string> slotList = new List<string>();				// Slots are special, just store a (string)int in list
		slotList.Add(DataLoaderTiers.GetSlotsUnlockedAtTier(tier).ToString());
		unlockHash.Add(AssetTypes.Slot, slotList);

		return unlockHash;
	}
}
