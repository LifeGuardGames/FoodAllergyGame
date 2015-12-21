using UnityEngine;
using System.Collections.Generic;

public class AnalyticsManager : Singleton<AnalyticsManager> {

	// When customer are leaving happy, what is their satisfaction?
	public void CustomerLeaveHappy(int satisfaction) {
		Mixpanel.SendEvent("Customer Leave Happy", new Dictionary<string, object>{
			{ "Customer Satisfaction " , satisfaction.ToString()}});
	}

	// When customers are not leaving happy, what is their state and type?
	public void CustomerLeaveAngry(CustomerTypes type, CustomerStates state) {
		Mixpanel.SendEvent("Customer Leave Angry:", new Dictionary<string, object> {
			{ "Customer Type:" , type.ToString() },
			{"Customer State:" , state.ToString()}});
	}

	public void EndGameDayReport(int totalCash, string currentEvent, int missingCustomers, float avgSatisfaction, int cashEarned,
		int medicCost, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		Mixpanel.SendEvent("End of Day Report:", new Dictionary<string, object> {
			{ "TotalCash:", totalCash*(1.0f) },
			{ "Event:" , currentEvent },
			{ "Missed Customers:", missingCustomers*1.0f },
			{ "Avg. Satisfaction:", avgSatisfaction },
			{ "Cash Earned:", cashEarned * 1.0f },
			{ "Cash Lost:", medicCost * 1.0f},
			{ "Medic Saved:", savedCustomers * 1.0f },
			{ "Attempted Rescues:", rescuesAttempted*1.0f },
			{ "Inspection Buttons Clicks:", inspectButtonClicked * 1.0f }});
	}

	public void EndGameUsageReport(int playAreaUses, int vipUses, int microwaveUses) {
		Mixpanel.SendEvent("End of Day Report:", new Dictionary<string, object> {
		{ "Play Area Uses:", playAreaUses * 1.0f },
		{ "VIP Uses:", vipUses*1.0f },
		{ "Microwave Uses:", microwaveUses*1.0f }});
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurant(float dayTimeLeft, int tier, string currentEvent, float difficultyLevel,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost) {
		Mixpanel.SendEvent("Quit Restaurant Incomplete:", new Dictionary<string, object> {
			{ "Time Left:", dayTimeLeft },
			{ "Tier:", tier*1.0f },
			{ "Event:" , currentEvent },
			{ "Difficulty Level:", difficultyLevel },
			{ "Missed Customers:", missingCustomers*1.0f },
			{ "Avg. Satisfaction:", averageSatisfication },
			{ "Cash Earned:", dayEarnedCash },
            { "Cash Lost:", medicCost*1.0f }});
	}

	// Tutorial funnel
	public void TutorialFunnel(string funnelString) {
		Mixpanel.SendEvent("Tutorial Flow:", new Dictionary<string, object>{
			{ "step", funnelString} });
	}

	// What customers are being spawned each day (discounts tutorial)?
	public void TrackCustomerSpawned(string customerID) {
		Mixpanel.SendEvent("Customer Spawn:", new Dictionary<string, object> {
			{ "Customer:" , customerID } });
	}

	// How often does users enter deco over time played?
	public void TrackSceneEntered(string sceneEntered) {
		Mixpanel.SendEvent("Scene Enter Count:", new Dictionary<string, object> {
			{ "Scene:" , sceneEntered }});
	}

	public void TrackDecoBought(string decoID) {
		Mixpanel.SendEvent("Item Bought:", new Dictionary<string, object> {
			{ "Item:" , decoID }});
	}

	// Menu planning food choices over time
	public void TrackMenuChoices(List<string> menuChoices) {
		foreach(string menuChoice in menuChoices) {
			Mixpanel.SendEvent("Menu Choices:", new Dictionary<string, object> {
				{ "Food:" , menuChoice }});
		}
	}

	// When do people quit the game?
    public void TrackGameQuitScene() {
		Mixpanel.SendEvent("Quit Game:" , new Dictionary<string, object> {
			{ "Scene:" , Application.loadedLevelName },
			{ "Sessions Played:" , DataManager.Instance.DaysInSession * 1.0f } });
	}
}
