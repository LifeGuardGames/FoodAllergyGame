using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ChallengeMenuManager : Singleton<ChallengeMenuManager> {
	public RectTransform challengeGrid;
	public GameObject challengeButtonPrefab;
	public ChallengeDescriptionController challengeDescription;
	public Transform topPosition;
	public Transform bottomPosition;

	void Start() {

		List<ImmutableDataChallenge> unorderedList = DataLoaderChallenge.GetDataList();
        List<ImmutableDataChallenge> challengeList = (from element in unorderedList
													  orderby element.ID ascending
													  select element).ToList();
		//Calculate level progress
		string challID = DataManager.Instance.GameData.Challenge.ChallengeUnlocked[DataManager.Instance.GameData.Challenge.ChallengeUnlocked.Count - 1];
		if(DataManager.Instance.GameData.Challenge.ChallengeProgress[challID] != ChallengeReward.None && DataManager.Instance.GameData.Challenge.ChallengeProgress[challID] != ChallengeReward.Stone) {
			DataManager.Instance.GameData.Challenge.ChallengeUnlocked.Add(challengeList[challengeList.IndexOf(DataLoaderChallenge.GetData(challID)) + 1].ID);
        }


		int regularChallengeCount = 0;
		GameObject challengeButton;
        foreach(ImmutableDataChallenge challengeData in challengeList) {
			if(challengeData.Tier <= TierManager.Instance.CurrentTier) {
				if(regularChallengeCount % 2 == 0) {
						if(challengeData.ChallengeType == ChallengeTypes.Character) {
							challengeButton = GameObjectUtils.AddChildGUI(bottomPosition.gameObject, GetCharacterShip(challengeData));
							challengeButton.GetComponent<ChallengeButton>().Init(challengeData, regularChallengeCount);
						}
						else {
							challengeButton = GameObjectUtils.AddChildGUI(bottomPosition.gameObject, challengeButtonPrefab);
							challengeButton.GetComponent<ChallengeButton>().Init(challengeData, regularChallengeCount);
						}
					}


				else {
						if(challengeData.ChallengeType == ChallengeTypes.Character) {
							challengeButton = GameObjectUtils.AddChildGUI(topPosition.gameObject, GetCharacterShip(challengeData));
							challengeButton.GetComponent<ChallengeButton>().Init(challengeData, regularChallengeCount);
					}
						else {
							challengeButton = GameObjectUtils.AddChildGUI(topPosition.gameObject, challengeButtonPrefab);
							challengeButton.GetComponent<ChallengeButton>().Init(challengeData, regularChallengeCount);
					}
				}
				
				regularChallengeCount++;
            }
		}
		// Adjust the grid width based on the height of the cell and spacing
		//float gridHeight = regularChallengeCount * (challengeGrid.cellSize.y + challengeGrid.spacing.y);
		//float gridWidth = regularChallengeCount * 500 + challengeGrid.sizeDelta.x;
        //challengeGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(gridWidth, challengeGrid.GetComponent<RectTransform>().sizeDelta.y);
		AudioManager.Instance.PlayClip("ChallengeShipEnter");
	}

	public void StartChallenge(string challengeID) {
		if(challengeID == "Challenge00") {
			AnalyticsManager.Instance.EpiPenGamePractice();
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.EPIPEN, additionalImageKey: "LoadingImageEpipen");
		}
		else if (DataLoaderChallenge.GetData(challengeID).ChallengeType == ChallengeTypes.Tutorial) {
			DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = challengeID;
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.RESTAURANT, showRandomTip: true);
		}
		else {
			DataManager.Instance.GameData.RestaurantEvent.CurrentChallenge = challengeID;
			LoadLevelManager.Instance.StartLoadTransition(SceneUtils.MENUPLANNING, showRandomTip: true);
		}
	}

	public void OnBackButtonClicked() {
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START, showRandomTip: true);
	}

	public void ShowPrompt(string challengeID) {
		if(DataManager.Instance.GameData.Challenge.ChallengeUnlocked.Contains(challengeID)) {
			challengeDescription.Populate(challengeID);
		}
	}

	public GameObject GetCharacterShip(ImmutableDataChallenge cDat) {
		GameObject Ship = new GameObject();
		switch(cDat.ID) {
			case "Challenge05":
				Debug.Log("working");
			//	Debug.Log((Resources.Load("ChallengeButtonAsteroid") as GameObject).name);
				Ship =  Resources.Load("ChallengeButtonAsteroid") as GameObject;
				break;
			case "Challenge15":
				Ship = (GameObject)Resources.Load("ChallengeButtonTiki") as GameObject;
				break;
			case "Challenge25":
				Ship = (GameObject)Resources.Load("ChallengeButtonSly") as GameObject;
				break;
			case "Challenge35":
				Ship = (GameObject)Resources.Load("ChallengeButtonCloud") as GameObject;
				break;
			case "Challenge40":
				Ship = (GameObject)Resources.Load("ChallengeButtonGabby") as GameObject;
				break;
			case "Challenge45":
				Ship = (GameObject)Resources.Load("ChallengeButtonSparky") as GameObject;
				break;
			case "Challenge50":
				Ship = (GameObject)Resources.Load("ChallengeButtonGrumbly") as GameObject;
				break;
			case "Challenge55":
				Ship = (GameObject)Resources.Load("ChallengeButtonPunchy") as GameObject;
				break;
			default:
				Ship = (GameObject)Resources.Load("ChallengeButtonAsteroid") as GameObject;
				break;
		}
		return Ship;
	}

	public void PlanetTapped() {
		LoadLevelManager.Instance.StartLoadTransition(SceneUtils.START);
		} 

}
