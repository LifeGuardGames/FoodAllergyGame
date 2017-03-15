using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class AnalyticsManager : Singleton<AnalyticsManager> {
	public Dictionary<string, object> SuperProperties = new Dictionary<string, object>();

	// When customer are leaving happy, what is their satisfaction?
	public void CustomerLeaveHappy(CustomerTypes type, int satisfaction) {
		Amplitude.Instance.logEvent("Customer Leave Happy", new Dictionary<string, object>{
			{"Customer Type: ", type.ToString()},
			{"Customer Satisfaction: ", satisfaction.ToString()},
			{"SuperProperties", SuperProperties}
		});
	}

	// When customers are not leaving happy, what is their state and type?
	public void CustomerLeaveAngry(CustomerTypes type, CustomerStates state) {
		Amplitude.Instance.logEvent("Customer Leave Angry", new Dictionary<string, object> {
			{"Customer Type: ", type.ToString()},
			{"Customer State: ", state.ToString()},
			{"SuperProperties", SuperProperties}
		});
	}

	public void EndGameDayReport(string currentEvent, int missingCustomers, float avgSatisfaction, int cashEarned,
		int medicCost, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		Amplitude.Instance.logEvent("End of Day Report", new Dictionary<string, object> {
			{"TotalCash: ", TierManager.Instance.CurrentTier * (1.0f)},
			{"Event: ", currentEvent},
			{"Missed Customers: ", missingCustomers * 1.0f},
			{"Avg. Satisfaction: ", avgSatisfaction},
			{"Cash Earned: ", cashEarned * 1.0f},
			{"Cash Lost: ", medicCost * 1.0f},
			{"Medic Saved: ", savedCustomers * 1.0f},
			{"Attempted Rescues: ", rescuesAttempted * 1.0f},
			{"Inspection Buttons Clicks: ", inspectButtonClicked * 1.0f},
			{ "SuperProperties", SuperProperties}
		});
	}

	public void EndChallengeReport(int score, string currentChallenge, int missingCustomers, float avgSatisfaction, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		Amplitude.Instance.logEvent("End of Challenge Report", new Dictionary<string, object> {
			{"Score: ", score * (1.0f)},
			{"Challenge: ", currentChallenge},
			{"Missed Customers: ", missingCustomers * 1.0f},
			{"Avg. Satisfaction: ", avgSatisfaction},
			{"Medic Saved:", savedCustomers * 1.0f},
			{"Attempted Rescues: ", rescuesAttempted * 1.0f},
			{"Inspection Buttons Clicks: ", inspectButtonClicked * 1.0f},
			{"SuperProperties", SuperProperties}
		});
	}

	public void VIPUsage(int vipUse) {
		Amplitude.Instance.logEvent("VIP Usage", new Dictionary<string, object>() {
			{"Vip Usage: ", vipUse},
			{"SuperProperties", SuperProperties}
		});
	}

	public void PlayAreaUsage(int playAreaUse) {
		Amplitude.Instance.logEvent("Play Area Usage", new Dictionary<string, object>() {
			{"PlayArea Usage: ", playAreaUse},
			{"SuperProperties", SuperProperties}
		});
	}

	public void EndGameUsageReport(int playAreaUses, int vipUses, int microwaveUses) {
		Amplitude.Instance.logEvent("End of Day Report", new Dictionary<string, object> {
			{"Play Area Uses: ", playAreaUses * 1.0f},
			{"VIP Uses: ", vipUses * 1.0f},
			{"Microwave Uses: ", microwaveUses * 1.0f},
			{"SuperProperties", SuperProperties}
		});
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurantArcade(float dayTimeLeft, int tier, string currentEvent, float difficultyLevel,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost, int restMode) {
		Amplitude.Instance.logEvent("Quit Restaurant Incomplete", new Dictionary<string, object> {
			{"Time Left: ", dayTimeLeft},
			{"Tier: ", tier * 1.0f},
			{"Event: ", currentEvent},
			{"Difficulty Level: ", difficultyLevel},
			{"Missed Customers: ", missingCustomers * 1.0f},
			{"Avg. Satisfaction: ", averageSatisfication},
			{"Cash Earned: ", dayEarnedCash},
			{"Cash Lost: ", medicCost * 1.0f },
			{"SuperProperties", SuperProperties},
			{"Rest Mode:", restMode }
		});
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurantChallenge(float dayTimeLeft, int tier, string currentChallenge,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost) {
		Amplitude.Instance.logEvent("Quit Restaurant Incomplete", new Dictionary<string, object> {
			{"Time Left: ", dayTimeLeft},
			{"Tier: ", tier * 1.0f},
			{"Challenge: ", currentChallenge},
			{"Missed Customers: ", missingCustomers * 1.0f},
			{"Avg. Satisfaction: ", averageSatisfication},
			{"Cash Earned: ", dayEarnedCash},
			{"Cash Lost: ", medicCost * 1.0f },
			{"SuperProperties", SuperProperties}
		});
	}

	// Tutorial funnel
	public void TutorialFunnel(string funnelString) {
		Amplitude.Instance.logEvent("Tutorial Flow" + funnelString);
	}

	public void NotificationFunnel() {
		Amplitude.Instance.logEvent("Notification Flow");
	}

	public void DayOneFunnel(string funnelStep) {
		Amplitude.Instance.logEvent("DayOne Flow " + funnelStep);
	}

	// How often does users enter deco over time played?
	public void TrackSceneEntered(string sceneEntered) {
		Amplitude.Instance.logEvent("Scene Enter Count", new Dictionary<string, object> {
			{"Scene: ", sceneEntered },
			{"SuperProperties", SuperProperties}
		});
	}

	public void TrackDecoBought(string decoID) {
		Amplitude.Instance.logEvent("Item Bought", new Dictionary<string, object> {
			{"Item: ", decoID},
			{"SuperProperties", SuperProperties}
		});
	}

	// Menu planning food choices over time
	public void TrackMenuChoices(List<string> menuChoices) {
		foreach(string menuChoice in menuChoices) {
			Amplitude.Instance.logEvent("Menu Choices", new Dictionary<string, object> {
				{"Food: ", menuChoice},
				{"SuperProperties", SuperProperties}
			});
		}
	}

	// When do people quit the game?
	public void TrackGameQuitScene() {
		TimeSpan timeInSession = System.DateTime.Now.Subtract(DataManager.Instance.GameData.Session.start);
		Amplitude.Instance.logEvent("Quit Game", new Dictionary<string, object> {
			{"Scene: ", SceneManager.GetActiveScene().name},
			{"Sessions Played: ", DataManager.Instance.DaysInSession * 1.0f},
			{"Time In Sesson: ", timeInSession.TotalMinutes},
			{"SuperProperties", SuperProperties}
		});

	}

	//Epi Pen Game
	public void EpiPenGameResultsAalytics(int attempts, int difficulty, string time) {
		Amplitude.Instance.logEvent("Epi Pen Game Results", new Dictionary<string, object> {
			{"Attempts: ", attempts},
			{"Difficulty: ", difficulty},
			{"Time Taken: ", time },
			{"SuperProperties", SuperProperties}
		});
	}

	public void MissedPiece(int wrongPiece) {
		Amplitude.Instance.logEvent("Wrong Piece", new Dictionary<string, object> {
			{"Piece: ", wrongPiece},
			{"SuperProperties", SuperProperties}
		});
	}

	public void EpiPenGamePractice() {
		Amplitude.Instance.logEvent("Epi Pen Game Practice", new Dictionary<string, object> {
			{"Is Practice Play: ", true},
			{"SuperProperties", SuperProperties}
		});
	}
	
	public void SendAge(string age) {
		Amplitude.Instance.logEvent("Age", new Dictionary<string, object> {
			{"Age: ", age},
			{"SuperProperties", SuperProperties}
		});
	}

	public void SkipComic(int page) {
		Amplitude.Instance.logEvent("Skipped Comic", new Dictionary<string, object> {
			{"Skipped Page: ", page}
		});
	}

	// Tracked if the ad has been seen
	public void AdImpression(string campaignName) {
		Amplitude.Instance.logEvent("Native Ads Impression", new Dictionary<string, object> {
			{"Name: ", campaignName}
		});
	}

	// Tracked if the user interacted with the ad at all (besides quitting)
	public void AdConversion(string campaignName, AdConversionType convertionType) {
		Amplitude.Instance.logEvent("Native Ads Conversion", new Dictionary<string, object> {
			{"Name: ", campaignName},
			{"Click: ", convertionType.ToString()}
		});
	}
}
