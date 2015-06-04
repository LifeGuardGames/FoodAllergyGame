using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	//StartMenu function - starts menu config - starts the meu builder found in food manager
	//StartGame function - begins the game with the current menu config starts the dayManager
	//RestartMenu function - restarts back at the menu builder 
	//RestartGame function - restarts resets the scene and stop all coroutines 
	//PauseGame function - pauses all corutines and movement
	//End Game - called by Day manager shows the results screen and grabs stats from the appropriate resources
	public bool isPaused;

	public void GameStart(){
		DayManager.Instance.StartDay();
	}

	public void GameOver(){
	}

	public void DayComplete(){

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
