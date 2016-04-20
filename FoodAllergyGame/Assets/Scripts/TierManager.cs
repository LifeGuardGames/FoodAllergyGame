using UnityEngine;
using System.Collections.Generic;

public class TierManager : Singleton<TierManager> {
	private string tierXMLPrefix = "Tier";          // Prefix of the tier xml keys, ie. "Tier04"

	private int oldTier;							// Cached old tier for use throughout game
	public int OldTier {
		get { return oldTier; }
	}

	private int currentTier;						// Cached tier for use throughout game
	public int CurrentTier {
		get{ return currentTier; }
	}

	private bool hasNewStarCore;
	public bool HasNewStarCore {
		get { return hasNewStarCore; }
	}

	private bool isNewUnlocksAvailable = false;
	public bool IsNewUnlocksAvailable {
		get{ return isNewUnlocksAvailable; }
	}

	private bool isTierUp = false;
	public bool IsTierUp {
		get { return isTierUp; }
	}

	private bool isPlayDecoTutorialNext = false;
	public bool IsPlayDecoTutorialNext {
		get{ return isPlayDecoTutorialNext; }
	}

	private Dictionary<AssetTypes, List<string>> currentTierUnlocks;	// A hash of all the types of unlocks with their tiers
	public Dictionary<AssetTypes, List<string>> CurrentTierUnlocks {
		get{ return currentTierUnlocks; }
	}

	// Recalculate the tier given a certain algorithm
	// NOTE: does NOT support multiple tiers!
	public void RecalculateTier() {
		isNewUnlocksAvailable = false;
		isTierUp = false;

        if(CashManager.Instance.TotalCash > 5800 && !DataManager.Instance.GameData.DayTracker.IsMoreCrates) {
			CashManager.Instance.OverrideTotalCash(5800);
        }
		oldTier = DataLoaderTiers.GetTierFromCash(CashManager.Instance.LastSeenTotalCash);
		currentTier = oldTier; // TODO Triple check this line
		int newTier = DataLoaderTiers.GetTierFromCash(CashManager.Instance.TotalCash);

		// If there is a change in tier, run logic
		// INVARIABLE: Tiers are maximum one above, never multiple tiers at once
		if(oldTier < newTier) {
			isTierUp = true;

			if(newTier - oldTier > 1) {
				Debug.LogError("Multiple tiers progressed, messes with unlock progression");
			}
			
			//deco new list compilation
			List<string> temp = DataLoaderDecoItem.GetBasicDecoIDUnlockedAtTier(newTier);
			List<string> specialTemp = DataLoaderDecoItem.GetSpecialDecoIDUnlockedAtTier(newTier);
			foreach(string id in specialTemp) {
				temp.Add(id);
			}
			foreach(string id in temp) {
				DataManager.Instance.GameData.Decoration.NewDeco.Add(id);
			}

			DataManager.Instance.GameData.Epi.HasPlayedEpiPenGameThisTier = false;
			DataManager.Instance.GameData.Epi.Difficulty++;
			currentTierUnlocks = GetAllUnlocksAtTier(newTier);

			// Check if the data structure has any unlocks
			foreach(KeyValuePair<AssetTypes, List<string>> hashEntry in currentTierUnlocks) {
				if(hashEntry.Key == AssetTypes.Slot) {  // Slot always has an entry, check value
					if(hashEntry.Value[0] != "0") {
						isNewUnlocksAvailable = true;
						break;
					}
				}
				else {
					if(hashEntry.Value.Count > 0) {
						isNewUnlocksAvailable = true;
						break;
					}
				}
			}

			if(isNewUnlocksAvailable) {
				// Add any tutorial overrides to be loaded next
				List<string> unlockedSpecialDecos = currentTierUnlocks[AssetTypes.DecoSpecial];
				if(unlockedSpecialDecos.Contains("VIPBasic")) {
					DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "TutDecoVIP";
					DataManager.Instance.GameData.Decoration.BoughtDeco.Add("VIPBasic","");
				}
				else if(unlockedSpecialDecos.Contains("PlayArea00")) {
					DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "TutDecoPlayArea";
					DataManager.Instance.GameData.Decoration.BoughtDeco.Add("PlayArea00", "");
				}
				else if(unlockedSpecialDecos.Contains("FlyThru00")) {
					DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "TutDecoFlyThru";
					DataManager.Instance.GameData.Decoration.BoughtDeco.Add("FlyThru00", "");
				}
			}
			currentTier = newTier;
		}

		// Print out tier
		Debug.Log("New tier:" + currentTier + "  ||  total cash:" + CashManager.Instance.TotalCash + "  ||  new unlocks? " + IsNewUnlocksAvailable);
	}

	public int GetMenuSlots(){
		return DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(currentTier)).MenuSlots;
	}

	// Loops through all previous events for a compiled list
	public List<string> GetEventsUnlocked(){
		List<string> eventsUnlocked = new List<string>();
		if(currentTier >= 0){
			for(int i = 0; i <= currentTier; i++){
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
		if(currentTier >= 0){
			for(int i = 0; i <= currentTier; i++){
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
		if(currentTier >= 0){
			for(int i = 0; i <= currentTier; i++){
				string[] assetsAtTier = DataLoaderTiers.GetData(tierXMLPrefix + StringUtils.FormatIntToDoubleDigitString(i)).StartArtAssets;
				foreach(string restaurantEvent in assetsAtTier){
					startArtAssets.Add(restaurantEvent);
				}
			}
		}
		return startArtAssets;
	}

	public string GetNewEvent(){
		List <string> eventList = GetEventsUnlocked();
		int rand = UnityEngine.Random.Range(0, eventList.Count);
		return eventList[rand];
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
		unlockHash.Add(AssetTypes.DecoSpecial, DataLoaderDecoItem.GetSpecialDecoIDUnlockedAtTier(tier));
		unlockHash.Add(AssetTypes.DecoBasic, DataLoaderDecoItem.GetBasicDecoIDUnlockedAtTier(tier) );
		unlockHash.Add(AssetTypes.Food, DataLoaderFood.GetIDUnlockedAtTier(tier));
		unlockHash.Add(AssetTypes.Challenge, DataLoaderChallenge.GetIDUnlockedAtTier(tier));

		List<string> slotList = new List<string>();				// Slots are special, just store a (string)int in list
		slotList.Add(DataLoaderTiers.GetSlotsUnlockedAtTier(tier).ToString());
		unlockHash.Add(AssetTypes.Slot, slotList);

		return unlockHash;
	}

	public void PrintAllUnlocksDebug() {
		for(int i = 0; i < 37; i++) {
			Debug.Log(i + " --------------");
			// Check if the data structure has any unlocks
			foreach(KeyValuePair<AssetTypes, List<string>> hashEntry in GetAllUnlocksAtTier(i)) {
				if(hashEntry.Key == AssetTypes.Slot) {  // Slot always has an entry, check value
					if(hashEntry.Value[0] != "0") {
						Debug.Log("   slot");
					}
				}
				else {
					if(hashEntry.Value.Count > 0) {
						foreach(string s in hashEntry.Value) {
							Debug.Log("   " + s);
						}
					}
				}
			}
		}
	}

	private void UpdateStarCoreCount() {
		if(DataManager.Instance.GameData.Challenge.StarCoresEarned > DataManager.Instance.GameData.Challenge.LastSeenStarCoresEarned) {
			hasNewStarCore = true;
			DataManager.Instance.GameData.Challenge.LastSeenStarCoresEarned = DataManager.Instance.GameData.Challenge.StarCoresEarned;
        }
		else {
			hasNewStarCore = false;
		}
	}
}
