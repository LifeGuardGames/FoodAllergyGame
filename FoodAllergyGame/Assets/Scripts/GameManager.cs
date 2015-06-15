using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

	//StartMenu function - starts menu config - starts the meu builder found in food manager
	//StartGame function - begins the game with the current menu config starts the dayManager
	//RestartMenu function - restarts back at the menu builder 
	//RestartGame function - restarts resets the scene and stop all coroutines 
	//PauseGame function - pauses all corutines and movement
	//End Game - called by Day manager shows the results screen and grabs stats from the appropriate resources
	public bool isPaused;
	public float cash;

	private static bool isCreated;

	void Awake(){
		// Make object persistent
		if(isCreated){
			// If There is a duplicate in the scene. delete the object and jump Awake
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		isCreated = true;
	}

	public void GameStart(){
		RestaurantManager.Instance.StartDay();
	}

	public void DayComplete(){
		//TODO Display Stats and replay games buttons
	}

	public float GetCash(){
		return cash;
	}

	public void CollectCash(float money){
		cash += money;
		GameObject.Find ("Score").GetComponent<Text>().text = "Score: " + cash.ToString();
	}

	public void TransitionScene(string scene){
		Application.LoadLevel(scene);
	}
}
