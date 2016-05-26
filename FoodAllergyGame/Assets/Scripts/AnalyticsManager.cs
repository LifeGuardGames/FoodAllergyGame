using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class AnalyticsManager : Singleton<AnalyticsManager> {

	// When customer are leaving happy, what is their satisfaction?
	public void CustomerLeaveHappy(CustomerTypes type, int satisfaction) {
		Mixpanel.SendEvent("Customer Leave Happy", new Dictionary<string, object>{
			{ "Customer Type: " , type.ToString() },
			{ "Customer Satisfaction: " , satisfaction.ToString()}});
	}


	// When customers are not leaving happy, what is their state and type?
	public void CustomerLeaveAngry(CustomerTypes type, CustomerStates state) {
		Mixpanel.SendEvent("Customer Leave Angry", new Dictionary<string, object> {
			{ "Customer Type: " , type.ToString() },
			{"Customer State: " , state.ToString()}});
	}

	public void EndGameDayReport(string currentEvent, int missingCustomers, float avgSatisfaction, int cashEarned,
		int medicCost, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		Mixpanel.SendEvent("End of Day Report", new Dictionary<string, object> {
			{ "TotalCash: ", TierManager.Instance.CurrentTier*(1.0f) },
			{ "Event: " , currentEvent },
			{ "Missed Customers: ", missingCustomers*1.0f },
			{ "Avg. Satisfaction: ", avgSatisfaction },
			{ "Cash Earned: ", cashEarned * 1.0f },
			{ "Cash Lost: ", medicCost * 1.0f},
			{ "Medic Saved: ", savedCustomers * 1.0f },
			{ "Attempted Rescues: ", rescuesAttempted*1.0f },
			{ "Inspection Buttons Clicks: ", inspectButtonClicked * 1.0f }});
	}

	public void EndChallengeReport(int score, string currentChallenge, int missingCustomers, float avgSatisfaction, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		Mixpanel.SendEvent("End of Challenge Report", new Dictionary<string, object> {
			{ "Score: ", score * (1.0f) },
			{ "Challenge: " , currentChallenge },
			{ "Missed Customers: ", missingCustomers * 1.0f },
			{ "Avg. Satisfaction: ", avgSatisfaction },
            { "Medic Saved:", savedCustomers * 1.0f },
			{ "Attempted Rescues: ", rescuesAttempted * 1.0f },
			{ "Inspection Buttons Clicks: ", inspectButtonClicked * 1.0f }
	});
	}

	public void VIPUsage(int vipUse) {
		Mixpanel.SendEvent("VIP Usage", new Dictionary<string, object>() {
			{"Vip Usage" , vipUse }
		});
	}

	public void PlayAreaUsage(int playAreaUse) {
		Mixpanel.SendEvent("Play Area Usage", new Dictionary<string, object>() {
			{"PlayArea Usage" , playAreaUse }
		});
	}

	public void EndGameUsageReport(int playAreaUses, int vipUses, int microwaveUses) {
		Mixpanel.SendEvent("End of Day Report", new Dictionary<string, object> {
		{ "Play Area Uses: ", playAreaUses * 1.0f },
		{ "VIP Uses: ", vipUses*1.0f },
		{ "Microwave Uses: ", microwaveUses*1.0f }});
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurantArcade(float dayTimeLeft, int tier, string currentEvent, float difficultyLevel,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost) {
		Mixpanel.SendEvent("Quit Restaurant Incomplete", new Dictionary<string, object> {
			{ "Time Left: ", dayTimeLeft },
			{ "Tier: ", tier*1.0f },
			{ "Event: " , currentEvent },
			{ "Difficulty Level: ", difficultyLevel },
			{ "Missed Customers: ", missingCustomers*1.0f },
			{ "Avg. Satisfaction: ", averageSatisfication },
			{ "Cash Earned: ", dayEarnedCash },
            { "Cash Lost: ", medicCost*1.0f }});
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurantChallenge(float dayTimeLeft, int tier, string currentChallenge,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost) {
		Mixpanel.SendEvent("Quit Restaurant Incomplete", new Dictionary<string, object> {
			{ "Time Left: ", dayTimeLeft },
			{ "Tier: ", tier*1.0f },
			{ "Challenge: " , currentChallenge },
			{ "Missed Customers: ", missingCustomers*1.0f },
			{ "Avg. Satisfaction: ", averageSatisfication },
			{ "Cash Earned: ", dayEarnedCash },
			{ "Cash Lost: ", medicCost*1.0f }});
	}

	// Tutorial funnel
	public void TutorialFunnel(string funnelString) {
		Mixpanel.SendEvent("Tutorial Flow" +funnelString);
	}

	public void NotificationFunnel() {
		Mixpanel.SendEvent("Notification Flow");
    }

	public void DayOneFunnel(string funnelStep) {
		Mixpanel.SendEvent("DayOne Flow " + funnelStep);
	}

	// What customers are being spawned each day (discounts tutorial)?
	public void TrackCustomerSpawned(string customerType) {
		Mixpanel.SendEvent("Customer Spawned", new Dictionary<string, object> {
			{"Customer: ", customerType } });
	}

	// How often does users enter deco over time played?
	public void TrackSceneEntered(string sceneEntered) {
		Mixpanel.SendEvent("Scene Enter Count", new Dictionary<string, object> {
			{ "Scene: " , sceneEntered }});
	}

	public void TrackDecoBought(string decoID) {
		Mixpanel.SendEvent("Item Bought", new Dictionary<string, object> {
			{ "Item: " , decoID }});
	}

	// Menu planning food choices over time
	public void TrackMenuChoices(List<string> menuChoices) {
		foreach(string menuChoice in menuChoices) {
			Mixpanel.SendEvent("Menu Choices", new Dictionary<string, object> {
				{ "Food: " , menuChoice }});
		}
	}

	// When do people quit the game?
	public void TrackGameQuitScene() {
		TimeSpan timeInSession = System.DateTime.Now.Subtract(DataManager.Instance.GameData.Session.start);
		Mixpanel.SendEvent("Quit Game", new Dictionary<string, object> {
			{ "Scene: " , SceneManager.GetActiveScene().name },
			{ "Sessions Played: " , DataManager.Instance.DaysInSession * 1.0f },
			{ "Time In Sesson: ", timeInSession.TotalMinutes} });

	}

	//Epi Pen Game
	public void EpiPenGameResultsAalytics(int attempts, int difficulty, string time) {
		Mixpanel.SendEvent("Epi Pen Game Results", new Dictionary<string, object> {
			{ "Attempts: " , attempts},
			{"Difficulty: ", difficulty },
			{"Time Taken: ", time },});
	}

	public void MissedPiece(int wrongPiece) {
		Mixpanel.SendEvent("Wrong Piece", new Dictionary<string, object> {
			{"Piece: ", wrongPiece }});
	}

	public void EpiPenGamePractice() {
		Mixpanel.SendEvent("Epi Pen Game Practice", new Dictionary< string, object>{
			{ "Is Practice Play: ", true}});
	}

	public void TimeSpentOnComicPage(float sec, int pageNum) {
		if(sec > 60) {
			Mixpanel.SendEvent("Page Finished", new Dictionary<string, object>{
				{ "Page: ", pageNum },
				{"Time: ", "60+"}});
		}
		else { 
			Mixpanel.SendEvent("Page Finished", new Dictionary<string, object>{
				{ "Page: ", pageNum },
				{"Time: ", sec }});
		}
	}

	public void SendAge(string _age) {
		Mixpanel.SendEvent("Age", new Dictionary<string, object> {
			{"Age: ", _age }});
	}
}
