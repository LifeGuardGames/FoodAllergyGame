using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class AnalyticsManager : Singleton<AnalyticsManager> {

	// When customer are leaving happy, what is their satisfaction?
	public void CustomerLeaveHappy(int satisfaction) {
		Analytics.CustomEvent("Customer Leave Happy", new Dictionary<string, object> {
			{"Customer Satisfaction", satisfaction},
		});
	}

	// When customers are not leaving happy, what is their state and type?
	public void CustomerLeaveAngry(CustomerTypes type, CustomerStates state) {
		Analytics.CustomEvent("Customer Leave Angry", new Dictionary<string, object> {
			{"Customer Type", type.ToString()},
            {"Customer State", state.ToString()}
		});
	}

	public void EndGameDayReport(int totalCash, string currentEvent, int missingCustomers, float avgSatisfaction, int cashEarned,
		int medicCost, int savedCustomers, int rescuesAttempted, int inspectButtonClicked) {
		Analytics.CustomEvent("End of Day Report", new Dictionary<string, object> {
			{"Days Played", DataManager.Instance.GameData.DayTracker.DaysPlayed},
			{"TotalCash", totalCash},
			{"Event", currentEvent},
			{"Missed Customers", missingCustomers},
			{"Avg. Satisfaction", avgSatisfaction},
			{"Cash Earned", cashEarned},
			{"Cash Lost", medicCost},
			{"Medic Saved", savedCustomers},
			{"Attempted Rescues", rescuesAttempted},
			{"Inspection Buttons Clicks", inspectButtonClicked}
		});
	}

	public void EndGameUsageReport(int playAreaUses, int vipUses, int microwaveUses) {
		Analytics.CustomEvent("End of Day Report", new Dictionary<string, object> {
			{"Days Played", DataManager.Instance.GameData.DayTracker.DaysPlayed},
			{"Play Area Uses", playAreaUses},
			{"VIP Uses", vipUses},
			{"Microwave Uses", microwaveUses},
		});
	}

	// Why do people quit the restaurant day incomplete?
	public void TrackGameDayInRestaurant(float dayTimeLeft, int tier, string currentEvent, float difficultyLevel,
		int missingCustomers, float averageSatisfication, int dayEarnedCash, int medicCost) {
		Analytics.CustomEvent("Quit Restaurant Incomplete", new Dictionary<string, object>{
			{"Time Left ", dayTimeLeft},
			{"Tier", tier},
			{"Event", currentEvent},
			{"Difficulty Level", difficultyLevel},
			{"Missed Customers", missingCustomers},
			{"Avg. Satisfaction", averageSatisfication},
			{"Cash Earned", dayEarnedCash},
			{"Cash Lost", medicCost},
		});
	}

	// Tutorial funnel
	public void TutorialFunnel(string funnelString) {
		Analytics.CustomEvent("Tutorial Flow", new Dictionary<string, object> {
			{funnelString, true}
		});
	}

	// What customers are being spawned each day (discounts tutorial)?
	public void TrackCustomerSpawned(string customerID) {
		Analytics.CustomEvent("Customer Spawn", new Dictionary<string, object> {
			{"Customer", customerID}
		});
	}

	// How often does users enter deco over time played?
	public void TrackSceneEntered(string sceneEntered) {
		Analytics.CustomEvent("Scene Enter Count", new Dictionary<string, object> {
			{"Scene", sceneEntered}
		});
	}

	public void TrackDecoBought(string decoID) {
		Analytics.CustomEvent("Item Bought", new Dictionary<string, object>{
			{"Item", decoID}
		});
	}

	// Menu planning food choices over time
	public void TrackMenuChoices(List<string> menuChoices) {
		Dictionary<string, object> dict = new Dictionary<string, object>();
		foreach(string menuChoice in menuChoices) {
			dict.Add("Food", menuChoice);
		}
		if(dict.Count > 10) {
			Debug.LogError("Analytics dictionary more than 10 parameters");
		}
		Analytics.CustomEvent("Menu", dict);
	}

	// When do people quit the game?
    public void TrackGameQuitScene() {
		Analytics.CustomEvent("Quit Game", new Dictionary<string, object> {
			{"Scene", Application.loadedLevelName},
			{"Sessions Played", DataManager.Instance.DaysInSession + "Days"}
		});
	}
}
