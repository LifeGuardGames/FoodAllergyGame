using UnityEngine;
using System.Collections;
using System;
using fastJSON;
using System.Collections.Generic;
using UnityEngine.Analytics;

/// <summary>
/// This class handles all game data. No game logic
/// Saves and loads data into player preference
/// </summary>
public class DataManager : Singleton<DataManager> {

	public event EventHandler<EventArgs> OnGameDataLoaded;
	public event EventHandler<EventArgs> OnGameDataSaved;

	private static bool isCreated;

	public bool isDebug = false;
	public bool IsDebug{
		get{ return isDebug; }
	}

	private GameData gameData;		// Super class that stores all the game data
	public GameData GameData{
		get{ return gameData; }
	}

	void Awake(){

		// Make object persistent
		if(isCreated){
			// If There is a duplicate in the scene. delete the object and jump Awake
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		isCreated = true;


		/////////
		PlayerPrefs.DeleteAll(); 	// Restart on new game
		/////////


		// JSON serializer setting
		JSON.Instance.Parameters.UseExtensions = false;
		JSON.Instance.Parameters.UseUTCDateTime = false;
		JSON.Instance.Parameters.UseOptimizedDatasetSchema = true;

		// Debug for an independent scene. Will initialize all data before other classes call DataManager
		if(isDebug){
			gameData = new GameData();
		}
		else{
			LoadGameData();
		}
	}

	/// <summary>
	/// Loads the game data from PlayerPrefs. If no game data is found from PlayerPrefs
	/// a new game data will be initiated. This function should only be called once the game loads
	/// </summary>
	public void LoadGameData(){
		if(gameData == null){
			string jsonString = PlayerPrefs.GetString("GameData", "");

			// Check if json string is actuall loaded and not empty
			if(!String.IsNullOrEmpty(jsonString)){
				gameData = JSON.Instance.ToObject<GameData>(jsonString);

				#if UNITY_EDITOR
				Debug.Log("Deserialized: " + jsonString);
				#endif
				
				Deserialized();
			}
			else{
				// Initiate game data here because none is found in the PlayerPrefs
				gameData = new GameData();
				Deserialized();
			}
		}
	}

	/// <summary>
	/// Serialize data into json string and store locally in PlayerPrefs
	/// </summary>
	public void SaveGameData(){
		//Data will not be saved if gameData is empty
		if(gameData != null){
			string jsonString = JSON.Instance.ToJSON(gameData);
			
			#if UNITY_EDITOR
			Debug.Log("SERIALIZED: " + jsonString);
			#endif
			
			PlayerPrefs.SetString("GameData", jsonString);
			Serialized();
		}
		else{
			Debug.LogError("gameData is null, so data cannot be serialized");
		}
	}

	public string GetEvent(){
		return gameData.RestaurantEvent.CurrentEvent;
	}

	/// <summary>
	/// Called when game data has been deserialized. Could be successful or failure
	/// </summary>
	private void Deserialized(){
		if(OnGameDataLoaded != null){
			OnGameDataLoaded(this, EventArgs.Empty);
		}
	}
	
	/// <summary>
	/// Called when game data has been serialized
	/// </summary>
	private void Serialized(){
		if(OnGameDataSaved != null){
			OnGameDataSaved(this, EventArgs.Empty);
		}
	}

	void OnApplicationPause(){
		Analytics.CustomEvent("Quit Game", new Dictionary<string, object>{
			{"Scene: ", Application.loadedLevelName} 
		});
	}

	#region Multi-scene Data Handling

	// This is called from both Restaurant and Deco scene, have this datamanager handle this
	public ImmutableDataDecoItem GetActiveDecoData(DecoTypes deco){
		return DataLoaderDecoItem.GetData(GameData.Decoration.ActiveDeco[deco]);
	}

	#endregion
}
