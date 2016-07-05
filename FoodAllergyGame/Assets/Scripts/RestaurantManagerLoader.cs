using UnityEngine;

public class RestaurantManagerLoader : MonoBehaviour {
	public GameObject RestArcade;
	public GameObject RestChallenge;

	void Start() {
		if(!string.IsNullOrEmpty(DataManager.Instance.GetChallenge())) {
			//Debug.Log(DataManager.Instance.GetChallenge());
			RestArcade.SetActive(false);
			RestaurantManagerChallenge.Instance.StartPhase();
		}
		else {
			RestChallenge.SetActive(false);
			RestaurantManagerArcade.Instance.StartPhase();
		}
	}
	
	// Called from PauseUIController
	public void QuitGame() {
		Time.timeScale = 1.0f;  // Remember to reset timescale!
		if(RestArcade.activeSelf) {
			int rand = Random.Range(0, 10);
			if(rand < DataManager.Instance.GameData.Epi.ChanceOfEpiGame) {
				DataManager.Instance.GameData.Epi.HasPlayedEpiPenGameThisTier = true;
				DataManager.Instance.GameData.Epi.ChanceOfEpiGame = 0;
				LoadLevelManager.Instance.StartLoadTransition(SceneUtils.EPIPEN, additionalTextKey: "LoadingKeyEpipen", additionalImageKey: "LoadingImageEpipen");
			}
			else {
				if(!DataManager.Instance.GameData.Epi.hasSeenEnding) {
					DataManager.Instance.GameData.Epi.ChanceOfEpiGame += 10;
				}
				else if(!DataManager.Instance.GameData.Epi.HasPlayedEpiPenGameThisTier ) {
					DataManager.Instance.GameData.Epi.ChanceOfEpiGame += 1;
				}
				LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: true);
			}
		}
		else if (DataManager.Instance.GetChallenge() != "ChallengeTut2"){
			DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: true);
		}
		else {
			DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = "";
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.COMICSCENE);
		}
	}
}
