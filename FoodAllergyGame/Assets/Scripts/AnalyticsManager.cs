using UnityEngine;
using GameAnalyticsSDK;
using System.Collections.Generic;

public class AnalyticsManager : Singleton<AnalyticsManager> {

	// When customer are leaving happy, what is their satisfaction?
	public void CustomerLeaveHappy(int satisfaction) {
		GA_Design.NewEvent("Customer Leave Happy:" + "Customer Satisfaction: " + satisfaction.ToString());
	}

	// When customers are not leaving happy, what is their state and type?
	public void CustomerLeaveAngry(CustomerTypes type, CustomerStates state) {
		GA_Design.NewEvent("Customer Leave Angry:" + "Customer Type:" + type.ToString());
		GA_Design.NewEvent("Customer Leave Angry:" + "Customer State:" + state.ToString());
	}

	public void EndGameDayReport(int totalCash, string currentEvent, int missingCustomers, float avgSatisfaction, int cashEarned,
		int medicCost, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		GA_Progression.NewEvent(GA_Progression.GAProgressionStatus.GAProgressionStatusComplete,"Days Played" ,  DataManager.Instance.GameData.DayTracker.DaysPlayed.ToString());
		GA_Design.NewEvent("End of Day Report:" + "TotalCash:", totalCash*(1.0f));
		GA_Design.NewEvent("End of Day Report:" + "Event:" + currentEvent);
		GA_Design.NewEvent("End of Day Report:" + "Missed Customers:", missingCustomers*1.0f);
		GA_Design.NewEvent("End of Day Report:" + "Avg. Satisfaction:", avgSatisfaction);
		GA_Design.NewEvent("End of Day Report:" + "Cash Earned:", cashEarned * 1.0f);
		if(medicCost > 0) {
			GA_Design.NewEvent("End of Day Report:" + "Cash Lost:", medicCost * 1.0f);
		}
		GA_Design.NewEvent("End of Day Report:" + "Medic Saved:", savedCustomers * 1.0f);
		GA_Design.NewEvent("End of Day Report:" + "Attempted Rescues:", rescuesAttempted*1.0f);
		GA_Design.NewEvent("End of Day Report:" + "Inspection Buttons Clicks:", inspectButtonClicked * 1.0f);
	}

	public void EndGameUsageReport(int playAreaUses, int vipUses, int microwaveUses) {
		GA_Design.NewEvent("End of Day Report:" + "Days Played:", DataManager.Instance.GameData.DayTracker.DaysPlayed*1.0f);
		GA_Design.NewEvent("End of Day Report:" + "Play Area Uses:", playAreaUses * 1.0f);
		GA_Design.NewEvent("End of Day Report:" + "VIP Uses:", vipUses*1.0f);
		GA_Design.NewEvent("End of Day Report:" + "Microwave Uses:", microwaveUses*1.0f);
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurant(float dayTimeLeft, int tier, string currentEvent, float difficultyLevel,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost) {
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Time Left:", dayTimeLeft);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Tier:", tier*1.0f);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Event:" + currentEvent);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Difficulty Level:", difficultyLevel);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Missed Customers:", missingCustomers*1.0f);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Avg. Satisfaction:", averageSatisfication);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Cash Earned:", dayEarnedCash);
		GA_Design.NewEvent("Quit Restaurant Incomplete:" + "Cash Lost:", medicCost*1.0f);
		GA_Progression.NewEvent(GA_Progression.GAProgressionStatus.GAProgressionStatusFail, "Days Played", DataManager.Instance.GameData.DayTracker.DaysPlayed.ToString());
	}

	// Tutorial funnel
	public void TutorialFunnel(string funnelString) {
		GA_Design.NewEvent("Tutorial Flow:" + funnelString);
	}

	// What customers are being spawned each day (discounts tutorial)?
	public void TrackCustomerSpawned(string customerID) {
		GA_Design.NewEvent("Customer Spawn:" + "Customer:" + customerID);
	}

	// How often does users enter deco over time played?
	public void TrackSceneEntered(string sceneEntered) {
		GA_Design.NewEvent("Scene Enter Count:" + "Scene:" + sceneEntered);
	}

	public void TrackDecoBought(string decoID) {
		GA_Design.NewEvent("Item Bought:" + "Item:" + decoID);
	}

	// Menu planning food choices over time
	public void TrackMenuChoices(List<string> menuChoices) {
		foreach(string menuChoice in menuChoices) {
			GA_Design.NewEvent("Menu Choices:" + "Food:" + menuChoice);
		}
	}

	// When do people quit the game?
    public void TrackGameQuitScene() {
		GA_Design.NewEvent("Quit Game:" + "Scene:" + Application.loadedLevelName);
		GA_Design.NewEvent("Quit Game:" + "Sessions Played:" , DataManager.Instance.DaysInSession * 1.0f);
	}
}
