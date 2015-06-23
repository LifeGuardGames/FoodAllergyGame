using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This class handles all game data. No game logic
/// Saves and loads data into player preference
/// </summary>
public class DataManager : Singleton<DataManager> {

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
}
