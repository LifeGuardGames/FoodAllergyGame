using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TierManager : Singleton<TierManager> {

	private string tierXMLPrefix = "Tier";	// Prefix of the tier xml keys, ie. "Tier04"

	private int tier;						// Cached tier for use throughout game
	public int Tier{
		get{ return tier; }
	}

	// Recalculate the tier given a certain algorithm, should be done on StartScene only
	public void RecalculateTier(){
		if(DataManager.Instance.IsDebug && Constants.GetDebugConstant<string>("Tier Number") != default(string)){
			tier = int.Parse(Constants.GetDebugConstant<string>("Tier Number"));
		}
		else{
			int progress = DataManager.Instance.GameData.Cash.TotalCash;
			if(tier+1 < progress / 1000){
				tier++;
				UnlockTier();
			}
			// Print out tier
			Debug.Log("Recalculated tier: " + tier + "     total cash: " + progress);
		}
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
			return eventList[Random.Range(0, eventList.Count)];
		}
	}

	public void UnlockTier(){
		//TODO unlock logic here
		if(tier == 1){
			ImmutableDataDecoItem decoData = DataLoaderDecoItem.GetData("PlayArea01");
			DataManager.Instance.GameData.Decoration.ActiveDeco.Remove(decoData.Type);
			DataManager.Instance.GameData.Decoration.ActiveDeco.Add(decoData.Type, decoData.ID);
			DataManager.Instance.GameData.Decoration.DecoTutQueue.Add("EventTP");
			DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;
		}
	}
}
