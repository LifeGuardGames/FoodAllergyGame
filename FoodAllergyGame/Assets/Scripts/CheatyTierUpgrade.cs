using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheatyTierUpgrade : MonoBehaviour {
	/// <summary>
	/// checks the current tier and assigns the total cash of the next tier up 
	/// we set the total cash as to not skip any tiers
	/// </summary>
	public void UpgradeTier() {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
		switch(TierManager.Instance.CurrentTier) {
			case 0:
				DataManager.Instance.GameData.Cash.TotalCash = 850;
				DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
                break;
			case 1:
				DataManager.Instance.GameData.Cash.TotalCash = 1500;
				break;
			case 2:
				DataManager.Instance.GameData.Tutorial.IsSpeDecoTutDone = true;
                DataManager.Instance.GameData.Cash.TotalCash = 2550;
                break;
			case 3:
				DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance = false;
                DataManager.Instance.GameData.Cash.TotalCash = 3600;
				break;
			case 4:
				DataManager.Instance.GameData.Cash.TotalCash = 4650;
				break;
			case 5:
				DataManager.Instance.GameData.Cash.TotalCash = 5800;
				break;
			case 6:
				DataManager.Instance.GameData.Cash.TotalCash = 7000;
				break;
			case 7:
				DataManager.Instance.GameData.Cash.TotalCash = 8300;
				break;
			case 8:
				DataManager.Instance.GameData.Cash.TotalCash = 10000;
				break;
			case 9:
				DataManager.Instance.GameData.Cash.TotalCash = 11300;
				break;
			case 10:
				DataManager.Instance.GameData.Cash.TotalCash = 12600;
				break;
			case 11:
				DataManager.Instance.GameData.Cash.TotalCash = 13900;
				break;
			case 12:
				DataManager.Instance.GameData.Cash.TotalCash = 15200;
				break;
			case 13:
				DataManager.Instance.GameData.Cash.TotalCash = 16500;
				break;
			case 14:
				DataManager.Instance.GameData.Cash.TotalCash = 17800;
				break;
			case 15:
				DataManager.Instance.GameData.Cash.TotalCash = 19100;
				break;
			case 16:
				DataManager.Instance.GameData.Cash.TotalCash = 20400;
				break;
			case 17:
				DataManager.Instance.GameData.Cash.TotalCash = 21700;
				break;
			case 18:
				DataManager.Instance.GameData.Cash.TotalCash = 23000;
				break;
			case 19:
				DataManager.Instance.GameData.Cash.TotalCash = 24300;
				break;
			case 20:
				DataManager.Instance.GameData.Cash.TotalCash = 25600;
				break;
			case 21:
				DataManager.Instance.GameData.Cash.TotalCash = 26900;
				break;
			case 22:
				DataManager.Instance.GameData.Cash.TotalCash = 28200;
				break;
			case 23:
				DataManager.Instance.GameData.Cash.TotalCash = 29500;
				break;
			case 24:
				DataManager.Instance.GameData.Cash.TotalCash = 30800;
				break;
			case 25:
				DataManager.Instance.GameData.Cash.TotalCash = 32100;
				break;
			case 26:
				DataManager.Instance.GameData.Cash.TotalCash = 33400;
				break;
			case 27:
				DataManager.Instance.GameData.Cash.TotalCash = 34700;
				break;
			case 28:
				DataManager.Instance.GameData.Cash.TotalCash = 36000;
				break;
			case 29:
				DataManager.Instance.GameData.Cash.TotalCash = 37300;
				break;
			case 30:
				DataManager.Instance.GameData.Cash.TotalCash = 38600;
				break;
			case 31:
				DataManager.Instance.GameData.Cash.TotalCash = 39900;
				break;
			case 32:
				DataManager.Instance.GameData.Cash.TotalCash = 41200;
				break;
			case 33:
				DataManager.Instance.GameData.Cash.TotalCash = 42500;
				break;
			case 34:
				DataManager.Instance.GameData.Cash.TotalCash = 43800;
				break;
			case 35:
				DataManager.Instance.GameData.Cash.TotalCash = 45100;
				break;
			default:
				break;
		}
		SceneManager.LoadScene(SceneUtils.START);

	}

	public void SkipOpeningTut() {
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
		DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
		SceneManager.LoadScene(SceneUtils.START);
	}

	public void SkipEverthing() {
		DataManager.
		DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
		DataManager.Instance.GameData.Tutorial.IsTutorial1Done = true;
		DataManager.Instance.GameData.Cash.LastSeenTotalCash = 45200;
		DataManager.Instance.GameData.Cash.TotalCash = 45700;
		Debug.Log(CashManager.Instance.TotalCash + " " + CashManager.Instance.LastSeenTotalCash);
		DataManager.Instance.GameData.Tutorial.IsSpeDecoTutDone =true;
		DataManager.Instance.GameData.Challenge.IsFirstTimeChallengeEntrance = false;
        DataManager.Instance.GameData.Tutorial.IsMenuPlanningFingerTutDone = true;
		DataManager.Instance.GameData.Tutorial.IsDecoFingerTutDone = true;
		DataManager.Instance.GameData.Decoration.IsFirstTimeEntrance = false;
		DataManager.Instance.GameData.RestaurantEvent.ShouldGenerateNewEvent = true;

		if(!DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey("VIPBasic")){
			DataManager.Instance.GameData.Decoration.BoughtDeco.Add("VIPBasic","");
		}
		if(!DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey("PlayArea00")){
			DataManager.Instance.GameData.Decoration.BoughtDeco.Add("PlayArea00", "");
		}
		if(!DataManager.Instance.GameData.Decoration.BoughtDeco.ContainsKey("FlyThru00")){
			DataManager.Instance.GameData.Decoration.BoughtDeco.Add("FlyThru00", "");
		}
		SceneManager.LoadScene(SceneUtils.START);
	}

	public void GetCash() {
		DataManager.Instance.GameData.Cash.CurrentCash += 10000;
		SceneManager.LoadScene(SceneUtils.START);
	}

	public void OnLoadEpipenButton() {
		SceneManager.LoadScene(SceneUtils.EPIPEN);
	}

	public void OnClearDataButton() {
		DataManager.Instance._DebugClearData();
	}
}