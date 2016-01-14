using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadLevelManager : Singleton<LoadLevelManager>{

	private static bool isCreated;

	public TweenToggleDemux loadDemux;
	public Text loadText;
	public Image loadImage;
	public FoodTipController foodTipController;
	public float foodTipWait = 1.3f; 			// How long to wait if the food tip controller is showing
	private string levelToLoad;

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

	/// <summary>
	/// Call this to start the transition
	/// </summary>
	/// <param name="levelName">Level to be loaded</param>
	public void StartLoadTransition(string levelName, string additionalTextKey = null, string additionalImageKey = null, bool showFoodTip = false){

		// Reset everything first
		loadText.text = "";
		loadImage.gameObject.SetActive(false);

		if(additionalTextKey != null) {
			loadText.text = LocalizationText.GetText(additionalTextKey);
		}
		if(additionalImageKey != null) {
			loadImage.gameObject.SetActive(true);
			loadImage.sprite = SpriteCacheManager.GetLoadingImageData(additionalImageKey);
		}
		if(showFoodTip){

		}

		levelToLoad = levelName;
		loadDemux.Show();
	}

	/// <summary>
	/// Call this to initiate loading (when ending tween is done), there will be a performance hit here
	/// </summary>
	public void ShowFinishedCallback(){
		if(levelToLoad != null){
			Application.LoadLevel(levelToLoad);
		}
		else{
			Debug.LogError("No level name specified");
		}
	}

	/// <summary>
	/// Hide the demux when the new level is loaded
	void OnLevelWasLoaded(){
		loadDemux.Hide();
	}
}
